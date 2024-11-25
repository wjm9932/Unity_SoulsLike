using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.IO;

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

    [Header("Save & Load")]
    [SerializeField] private bool allowLoad;

    [Header("Pause Menu")]
    public GameObject pauseMenu;

    [Header("Inventory")]
    public GameObject inventoryUI;

    [Header("Quest")]
    public GameObject questLogUI;
    public GameObject questDialogueUI;
    [SerializeField] private QuestInfoSO skillIsOnQuest;
    public bool isSkillOn { get; private set; }

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
    [SerializeField] private float maxSlopeAngle;
    public RaycastHit slopeHit;

    [Header("Attack")]
    public GameObject slash;
    public ParticleSystem chargingEffect;


    [Header("Player FootStep Sound")]
    [SerializeField] private List<AudioClip> tileFootStepClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> groundFootStepClips = new List<AudioClip>();
    private List<AudioClip> currentFootStepClips = new List<AudioClip>();
    private AudioSource playerFootStepSoundSource;
    public SoundManager.BackGroundMusic musicType;

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
        set
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

    private void OnEnable()
    {
        QuestManager.Instance.onFinishQuest += EnableSkill;
    }

    private void OnDisable()
    {
        QuestManager.Instance.onFinishQuest -= EnableSkill;
    }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        isSkillOn = false;
        chargingEffect.Stop();
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
        LoadData();

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
        if(Input.GetKeyDown(KeyCode.L))
        {
            var a = followCamera.GetComponent<CameraShake>();
            a.ShakeCamera();
        }
        if (isDead == false)
        {
            rb.useGravity = !IsOnSlope();
            playerMovementStateMachine.Update();
            uiStateMachine.Update();
            
            ResetCameraPosition();
            Unlock();
            RecoverStamina();
        }

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
            staminaRecoverCoolTime = targetStaminaRecoverCoolTime;
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

    public void ChangeSoundEffect(SoundManager.BackGroundMusic type)
    {
        musicType = type;

        switch (type)
        {
            case SoundManager.BackGroundMusic.BOSS:
            case SoundManager.BackGroundMusic.DUNGEON:
                currentFootStepClips = tileFootStepClips;
                break;
            case SoundManager.BackGroundMusic.OUTSIDE:
                currentFootStepClips = groundFootStepClips;
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
        base.Die();
        playerMovementStateMachine.ChangeState(playerMovementStateMachine.dieState);
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;

        yield return new WaitForSeconds(4f);

        ResetPlayer();
    }

    private void ResetPlayer()
    {
        health = maxHealth;
        stamina = maxStamina;
        playerMovementStateMachine.ChangeState(playerMovementStateMachine.idleState);
        transform.position = GetComponent<PlayerCheckPoint>().checkPointPosition;
        SoundManager.Instance.ChangeBackGroundMusic(SoundManager.BackGroundMusic.DUNGEON);
        GetComponent<Collider>().enabled = true;
        rb.isKinematic = false;
        isDead = false;
    }

    private void ResetCameraPosition()
    {
        if (playerMovementStateMachine.currentState != playerMovementStateMachine.sprintState)
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, originCameraTrasform, 2f * Time.deltaTime);
        }
    }
    private void Unlock()
    {
        if (input.isInteracting == true && uiStateMachine.currentState is OpenState == false)
        {
            playerEvents.Unlock();
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private PlayerSaveData GetPlayerData()
    {
        return new PlayerSaveData(transform.position, transform.rotation, health, maxHealth, stamina, musicType, GetComponent<PlayerCheckPoint>().checkPointPosition);
    }
    private void SaveData()
    {
        PlayerSaveData playerSaveData = GetPlayerData();
        string jsonData = JsonUtility.ToJson(playerSaveData, true);
        string path = Path.Combine(Application.dataPath, "playerData");
        File.WriteAllText(path, jsonData);
    }

    private void LoadData()
    {
        string path = Path.Combine(Application.dataPath, "playerData");

        if (allowLoad)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning("No player data found. Initializing default values.");
                InitializeDefaultPlayerData();
            }
            else
            {
                string jsonData = File.ReadAllText(path);
                PlayerSaveData playerSaveData = JsonUtility.FromJson<PlayerSaveData>(jsonData);

                transform.position = playerSaveData.playerPosition;
                transform.rotation = playerSaveData.playerRotation;

                maxHealth = playerSaveData.maxHealth;
                health = playerSaveData.currentHealth;
                stamina = playerSaveData.currentStamina;
                musicType = playerSaveData.musicType;
                hpBar.SetStatusBarSize(maxHealth);

                ChangeSoundEffect(musicType);
                SoundManager.Instance.ChangeBackGroundMusic(musicType);
            }
        }
        else
        {
            InitializeDefaultPlayerData();
        }
    }

    private void InitializeDefaultPlayerData()
    {
        health = 30f;
        stamina = maxStamina;
    }

    private void EnableSkill(Quest quest)
    {
        if(quest.info.id == skillIsOnQuest.id)
        {
            isSkillOn = true;
            QuestManager.Instance.onFinishQuest -= EnableSkill;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Resume()
    {
        uiStateMachine.ChangeState(uiStateMachine.closeState);
    }
}
