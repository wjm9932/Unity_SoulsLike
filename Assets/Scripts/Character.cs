using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class Character : LivingEntity
{
    public CinemachineVirtualCamera lockOnCamera;
    public CinemachineFreeLook lockOffCamera;
    public Transform lockOnCameraPosition;
    public TrailRenderer swordEffect;
    public LayerMask whatIsGround;
    public LayerMask enemy;

    public GameObject tempTarget;
    public Animator animator { get; private set; }
    public Camera followCamera { get; private set; }
    public Rigidbody rb { get; private set; }
    public float playerHeight { get; private set; }
    public PlayerInput input { get; private set; }

    private PlayerMovementStateMachine playerMovementStateMachine;
    // Start is called before the first frame update
    private void Awake()
    {
        canBeDamaged = true;
        CameraStateMachine.Initialize(this);
        playerMovementStateMachine = new PlayerMovementStateMachine(this);
    }
    void Start()
    {
        followCamera = Camera.main;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerHeight = GetComponent<CapsuleCollider>().height;
        input = GetComponent<PlayerInput>();

        playerMovementStateMachine.ChangeState(playerMovementStateMachine.idleState);
        CameraStateMachine.Instance.ChangeState(CameraStateMachine.Instance.cameraLockOffState);
    }

    // Update is called once per frame
    void Update()
    {
        playerMovementStateMachine.Update();
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
    private void OnAnimationExitEvent()
    {
        playerMovementStateMachine.OnAnimationExitEvent();
    }
    private void OnAnimationTransitionEvent()
    {
        playerMovementStateMachine.OnAnimationTransitionEvent();
    }
}
