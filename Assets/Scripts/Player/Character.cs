using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.TextCore.Text;
using UnityEditorInternal;

public class Character : LivingEntity
{
    public GameObject inventoryUI;

    public Image hpBar;


    public CinemachineVirtualCamera lockOnCamera;
    public CinemachineFreeLook lockOffCamera;
    public Transform lockOnCameraPosition;
    public Transform camEyePos;
    public Transform leftHandPos;
    public TrailRenderer swordEffect;
    public LayerMask whatIsGround;
    public LayerMask enemy;
    public float walkSpeed;
    public float sprintSpeed;

    public UI.Item clickedItem { get; private set; }

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


    private Inventory inventory;

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
        health = 50f;
        hpBar.fillAmount = health / maxHealth;

        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerHeight = GetComponent<CapsuleCollider>().height;
        input = GetComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();

        playerMovementStateMachine.ChangeState(playerMovementStateMachine.idleState);
        uiStateMachine.ChangeState(uiStateMachine.closeState);
        CameraStateMachine.Instance.ChangeState(CameraStateMachine.Instance.cameraLockOffState);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerMovementStateMachine.currentState);
        rb.useGravity = !IsOnSlope();
        playerMovementStateMachine.Update();
        uiStateMachine.Update();
        CameraStateMachine.Instance.Update();


        //input.IsClickItemInInventory(OnClickItem);

        /************************************Test********************************************************/
        //if(Input.GetKeyDown(KeyCode.Q) == true) // Quest
        //{
        //    closeWindow = CloseQuestWindow;
        //}

        //if (Input.GetKeyDown(KeyCode.I) == true) // Inventory
        //{
        //    closeWindow = CloseInventoryWindow;
        //}

        //if(Input.GetKeyDown(KeyCode.Escape) == true)
        //{
        //    closeWindow?.Invoke();
        //}
        /************************************Test********************************************************/
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
    private void OnAnimationExitEvent()
    {
        playerMovementStateMachine.OnAnimationExitEvent();
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
        if (Physics.Raycast(GetPlayerPosition(), Vector3.down, out slopeHit, playerHeight * 0.5f + 0.1f) == true)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && !Mathf.Approximately(angle, 0f);
        }

        return false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemySword")
        {
            if(playerMovementStateMachine.currentState != playerMovementStateMachine.hitState)
            {
                LivingEntity enemy = other.transform.root.GetComponent<LivingEntity>();

                if (enemy.canAttack == true && this.canBeDamaged == true)
                {
                    animator.SetTrigger("Hit");
                    playerMovementStateMachine.ChangeState(playerMovementStateMachine.hitState);

                    var hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = (transform.position - hitPoint).normalized;
                    EffectManager.Instance.PlayHitEffect(hitPoint, hitNormal, other.transform, EffectManager.EffectType.Flesh);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        UX.Item item = other.gameObject.GetComponent<UX.Item>();

        if (item != null)
        {
            inventory.AddItem(item);
            Destroy(item.gameObject);
        }
    }

    public bool OnClickItem()
    {
        clickedItem = inventory.GetItemUI(); 

        if (clickedItem != null)
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
