using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{

    public Animator animator { get; private set; }
    public Camera followCamera { get; private set; }
    public Rigidbody rb { get; private set; }
    public float playerHeight { get; private set; }
    public PlayerInput input { get; private set; }
    public LayerMask whatIsGround;

    private PlayerMovementStateMachine playerMovementStateMachine;
    // Start is called before the first frame update
    private void Awake()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        playerMovementStateMachine.Update();
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