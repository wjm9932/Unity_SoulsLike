using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

[RequireComponent(typeof(PlayerEvent))]
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

    [SerializeField] private float maxStamina;

    [Header("Inventory")]
    public GameObject inventoryUI;

    [Header("Quest")]
    public GameObject questLogUI;
    public GameObject questDialogueUI;

    [Header("Status Bar")]
    public PlayerStatusBar hpBar;
    public PlayerStatusBar staminaBar;

    [Header("Camera")]
    public CinemachineVirtualCamera followCamera;
    public Transform cameraTransform;
    public Transform camEyePos;
    public Vector3 originCameraTrasform { get; private set; }

    [Header("Layer Mask")]
    public LayerMask whatIsGround;
    public LayerMask enemyMask;
    public LayerMask lockOnCameraObstacleLayer;

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    [SerializeField]
    private float maxSlopeAngle;
    public RaycastHit slopeHit;


    [Header("Player FootStep Sound")]
    [SerializeField] private List<AudioClip> tileFootStepClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> groundFootStepClips = new List<AudioClip>();
    private List<AudioClip> currentFootStepClips = new List<AudioClip>();
    private AudioSource playerFootStepSoundSource;

    [Space]
    public Transform leftHandPos;
    public TrailRenderer swordEffect;
    public Transform arrowHitPositionParent;
    public UsableItem toBeUsedItem { get; private set; }
    public Camera mainCamera { get; private set; }
    public Rigidbody rb { get; private set; }
    public float playerHeight { get; private set; }
    public PlayerInput input { get; private set; }
    public PlayerMovementStateMachine playerMovementStateMachine { get; private set; }
    public UIStateMachine uiStateMachine { get; private set; }

    public Inventory inventory { get; private set; }
    public PlayerEvent playerEvents { get; private set; }
    public BuffManager playerBuff { get; private set; }
    public bool isInDungeon { get; private set; }
    public float buffDamage { get; set; }
    public float buffStaminaPercent { get; set; }

    [HideInInspector] public float cameraTargetYaw;
    [HideInInspector] public float cameraTargetPitch;

    public const float targetStaminaRecoverCoolTime = 1.5f;
    [HideInInspector] public float staminaRecoverCoolTime;
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

    public override float maxHealth
    {
        set
        {
            base.maxHealth = value;
            hpBar.UpdateStatusBar(health, maxHealth);
        }
    }



    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        currentFootStepClips = groundFootStepClips;
        originCameraTrasform = cameraTransform.transform.localPosition;
        staminaRecoverCoolTime = 0f;
        canBeDamaged = true;
        CameraStateMachine.Initialize(this);
        uiStateMachine = new UIStateMachine(this);
        playerMovementStateMachine = new PlayerMovementStateMachine(this);
    }
    protected override void Start()
    {
        health = 30f;
        stamina = maxStamina;

        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        playerHeight = GetComponent<CapsuleCollider>().height;
        input = GetComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
        playerEvents = GetComponent<PlayerEvent>();
        playerBuff = GetComponent<BuffManager>();
        playerFootStepSoundSource = GetComponent<AudioSource>();

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

        if (playerMovementStateMachine.currentState != playerMovementStateMachine.sprintState)
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, originCameraTrasform, 2f * Time.deltaTime);
        }

        CameraStateMachine.Instance.Update();

        if (input.isInteracting == true)
        {
            playerEvents.Unlock();
        }
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
    private void OnAnimationTransitionEvent(AnimationEvent ev)
    {
        if (ev.animatorClipInfo.weight >= 0.5f)
        {
            playerMovementStateMachine.OnAnimationTransitionEvent();
        }
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
                    if (playerMovementStateMachine.currentState != playerMovementStateMachine.dieState)
                    {
                        playerMovementStateMachine.ChangeState(playerMovementStateMachine.hitState);
                    }

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

                    if (playerMovementStateMachine.currentState != playerMovementStateMachine.dieState)
                    {
                        playerMovementStateMachine.ChangeState(playerMovementStateMachine.hitState);
                    }

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
        float actualCost = cost * (1 - buffStaminaPercent);
        if (stamina < actualCost)
        {
            TextManager.Instance.PlayNotificationText("Not enough stamina!");
            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ALERT, 0.2f);
            return false;
        }
        else
        {
            stamina -= actualCost;
            return true;
        }
    }
    public override void SetDamage(float damage)
    {
        this.damage = damage + buffDamage;
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

    public void ChangeFootStepSound(FootStepSoundType type)
    {
        switch (type)
        {
            case FootStepSoundType.TILE:
                currentFootStepClips = tileFootStepClips;
                isInDungeon = true;
                break;
            case FootStepSoundType.GROUND:
                currentFootStepClips = groundFootStepClips;
                isInDungeon = false;
                break;
        }
    }

    private void PlayFootStepSound(AnimationEvent ev)
    {
        if (ev.animatorClipInfo.weight >= 0.5f)
        {
            int index = Random.Range(0, currentFootStepClips.Count);
            playerFootStepSoundSource.PlayOneShot(currentFootStepClips[index]);
        }
    }

    public override void Die()
    {
        playerMovementStateMachine.ChangeState(playerMovementStateMachine.dieState);

        base.Die();
    }
}
