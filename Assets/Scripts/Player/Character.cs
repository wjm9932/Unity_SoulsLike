using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.TextCore.Text;
using UnityEditorInternal;

[RequireComponent(typeof(PlayerQuestEvent))]
[RequireComponent(typeof(Inventory))]
public class Character : LivingEntity
{
    public UsableItem quickSlot
    {
        get
        {
            toBeUsedItem = inventory.quickSlot;
            return toBeUsedItem;
        }
    }

    [Header("Quest")]
    public GameObject inventoryUI;
    public GameObject questLogUI;
    public GameObject questDialogueUI;

    [Header("Status Bar")]
    public PlayerStatusBar hpBar;
    public PlayerStatusBar staminaBar;

    [Header("Camera")]
    public CinemachineVirtualCamera lockOnCamera;
    public CinemachineFreeLook lockOffCamera;
    public Transform lockOnCameraPosition;
    public Transform camEyePos;

    [Header("Layer Mask")]
    public LayerMask whatIsGround;
    public LayerMask enemyMask;
    public LayerMask lockOnCameraTargetLayer;

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    [SerializeField]
    private float maxSlopeAngle;
    public RaycastHit slopeHit;

    [Space]
    public Transform leftHandPos;
    public TrailRenderer swordEffect;
    public Transform arrowHitPositionParent;
    public UsableItem toBeUsedItem { get; private set; }
    public Animator animator { get; private set; }
    public Camera mainCamera { get; private set; }
    public Rigidbody rb { get; private set; }
    public float playerHeight { get; private set; }
    public PlayerInput input { get; private set; }
    public PlayerMovementStateMachine playerMovementStateMachine { get; private set; }
    public UIStateMachine uiStateMachine { get; private set; }


    public Inventory inventory { get; private set; }
    public PlayerQuestEvent playerEvents { get; private set; }

    public const float targetStaminaRecoverCoolTime = 1.5f;
    public float staminaRecoverCoolTime;
    private float maxStamina;
    private float _stamina;
    public float stamina
    {
        get
        {
            return _stamina;
        }
        private set
        {
            _stamina = value;
            staminaBar.UpdateStatusBar(stamina, maxStamina);
        }
    }

    public override float health
    {
        protected set
        {
            base.health = value;
            hpBar.UpdateStatusBar(health, maxHealth);
        }
    }


    // Start is called before the first frame update
    private void Awake()
    {
        maxStamina = 100f;
        canBeDamaged = true;
        CameraStateMachine.Initialize(this);
        uiStateMachine = new UIStateMachine(this);
        playerMovementStateMachine = new PlayerMovementStateMachine(this);
    }
    void Start()
    {
        health = 100f;
        stamina = 40f;

        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerHeight = GetComponent<CapsuleCollider>().height;
        input = GetComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
        playerEvents = GetComponent<PlayerQuestEvent>();

        playerMovementStateMachine.ChangeState(playerMovementStateMachine.idleState);
        uiStateMachine.ChangeState(uiStateMachine.closeState);
        CameraStateMachine.Instance.ChangeState(CameraStateMachine.Instance.cameraLockOffState);
    }

    // Update is called once per frame
    void Update()
    {
        RecoverStamina();

        rb.useGravity = !IsOnSlope();
        playerMovementStateMachine.Update();
        uiStateMachine.Update();

        CameraStateMachine.Instance.Update();
    }
    private void FixedUpdate()
    {
        playerMovementStateMachine.PhysicsUpdate();
        CameraStateMachine.Instance.PhysicsUpdate();
    }
    private void LateUpdate()
    {
        playerMovementStateMachine.LateUpdate();
        CameraStateMachine.Instance.LateUpdate();
    }
    private void OnAnimationEnterEvent()
    {
        playerMovementStateMachine.OnAnimationEnterEvent();
    }
    private void OnAnimationExitEvent(AnimationEvent ev)
    {
        if (ev.animatorClipInfo.weight >= 0.5f)
        {
            playerMovementStateMachine.OnAnimationExitEvent();
        }
    }
    private void OnAnimationTransitionEvent()
    {
        playerMovementStateMachine.OnAnimationTransitionEvent();
    }
    private void OnAnimatorIK()
    {
        playerMovementStateMachine.OnAnimatorIK();
    }
    public Vector3 GetPlayerPosition()
    {
        return new Vector3(transform.position.x, transform.position.y + playerHeight / 2, transform.position.z);
    }

    public bool IsOnSlope()
    {
        if (Physics.Raycast(GetPlayerPosition(), Vector3.down, out slopeHit, playerHeight * 0.5f + 0.1f, whatIsGround) == true)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && !Mathf.Approximately(angle, 0f);
        }

        return false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemySword") == true)
        {
            if (playerMovementStateMachine.currentState is InteractState == true)
            {
                return;
            }

            LivingEntity enemy = other.transform.root.GetComponent<LivingEntity>();

            if (enemy.canAttack == true && enemy != null)
            {
                if (ApplyDamage(enemy) == true)
                {
                    playerMovementStateMachine.ChangeState(playerMovementStateMachine.hitState);

                    var hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = (hitPoint - transform.position).normalized;
                    EffectManager.Instance.PlayHitEffect(hitPoint, hitNormal, transform, EffectManager.EffectType.Flesh);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow") == true)
        {
            if (playerMovementStateMachine.currentState is InteractState == true)
            {
                return;
            }

            LivingEntity enemy = other.GetComponent<Arrow>().parent;

            if (enemy != null)
            {
                if (ApplyDamage(enemy) == true)
                {
                    other.GetComponent<Arrow>().rb.isKinematic = true;
                    other.GetComponent<Arrow>().arrowEffect.enabled = false;
                    other.GetComponent<Arrow>().enabled = false;
                    other.GetComponent<Collider>().enabled = false;
                    other.transform.SetParent(arrowHitPositionParent);
                    other.transform.position = arrowHitPositionParent.position;

                    playerMovementStateMachine.ChangeState(playerMovementStateMachine.hitState);

                    var hitPoint = other.transform.position;
                    Vector3 hitNormal = (hitPoint - transform.position).normalized;
                    EffectManager.Instance.PlayHitEffect(hitPoint, hitNormal, transform, EffectManager.EffectType.Flesh);
                }
            }
        }
    }

    public bool OnClickItem()
    {
        toBeUsedItem = inventory.GetItemUI();

        if (toBeUsedItem != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UseStamina(float cost)
    {
        if (stamina < cost)
        {
            return false;
        }
        else
        {
            stamina -= cost;
            return true;
        }
    }

    public void KillLivingEntity(EntityType type)
    {
        playerEvents.KillEnemy(type);
    }


    void RecoverStamina()
    {
        if (staminaRecoverCoolTime > 0)
        {
            staminaRecoverCoolTime -= Time.deltaTime;
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += 20f * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }
        }
    }

    /************************************Test********************************************************/
    private void CloseQuestWindow()
    {
        Debug.Log("Close QuestWindow");
    }

    private void CloseInventoryWindow()
    {
        Debug.Log("Close InventoryWindow");
    }
    /************************************Test********************************************************/
}
