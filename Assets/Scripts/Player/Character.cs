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

    public GameObject inventoryUI;
    public Image hpBar;
    public CinemachineVirtualCamera lockOnCamera;
    public CinemachineFreeLook lockOffCamera;

    public Transform lockOnCameraPosition;
    public Transform camEyePos;
    public Transform leftHandPos;

    public TrailRenderer swordEffect;

    public LayerMask whatIsGround;
    public LayerMask enemyMask;
    public LayerMask lockOnCameraTargetLayer;

    public float walkSpeed;
    public float sprintSpeed;

    public UsableItem toBeUsedItem { get; private set; }

    public Animator animator { get; private set; }
    public Camera mainCamera { get; private set; }
    public Rigidbody rb { get; private set; }
    public float playerHeight { get; private set; }
    public PlayerInput input { get; private set; }
    public PlayerMovementStateMachine playerMovementStateMachine { get; private set; }
    public UIStateMachine uiStateMachine { get; private set; }

    [SerializeField]
    private float maxSlopeAngle;

    public RaycastHit slopeHit;


    public Inventory inventory { get; private set; }
    public PlayerQuestEvent eventManager { get; private set; }

    public override float health
    {
        protected set
        {
            base.health = value;
            hpBar.fillAmount = health / maxHealth;
        }
    }


    // Start is called before the first frame update
    private void Awake()
    {
        canBeDamaged = true;
        CameraStateMachine.Initialize(this);
        uiStateMachine = new UIStateMachine(this);
        playerMovementStateMachine = new PlayerMovementStateMachine(this);
    }
    void Start()
    {
        health = 10f;

        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerHeight = GetComponent<CapsuleCollider>().height;
        input = GetComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
        eventManager = GetComponent<PlayerQuestEvent>();

        playerMovementStateMachine.ChangeState(playerMovementStateMachine.idleState);
        uiStateMachine.ChangeState(uiStateMachine.closeState);
        CameraStateMachine.Instance.ChangeState(CameraStateMachine.Instance.cameraLockOffState);
    }

    // Update is called once per frame
    void Update()
    {
        rb.useGravity = !IsOnSlope();

        playerMovementStateMachine.Update();
        uiStateMachine.Update();

        CameraStateMachine.Instance.Update();
    }
    private void FixedUpdate()
    {
        playerMovementStateMachine.PhysicsUpdate();
    }
    private void LateUpdate()
    {
        playerMovementStateMachine.LateUpdate();
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
            if(playerMovementStateMachine.currentState is InteractState == true)
            {
                return;
            }

            LivingEntity enemy = other.transform.root.GetComponent<LivingEntity>();

            if (enemy.canAttack == true && enemy != null)
            {
                if (ApplyDamage(enemy.damage) == true)
                {
                    playerMovementStateMachine.ChangeState(playerMovementStateMachine.hitState);

                    var hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = (transform.position - hitPoint).normalized;
                    EffectManager.Instance.PlayHitEffect(hitPoint, hitNormal, other.transform, EffectManager.EffectType.Flesh);
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
