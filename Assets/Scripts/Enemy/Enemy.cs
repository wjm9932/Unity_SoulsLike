using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public NavMeshAgent navMesh { get; private set; }
    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }
    public RaycastHit slopeHit;

    [SerializeField]
    private Character character;
    [SerializeField]
    private float maxSlopeAngle;
    private EnemyBehaviorStateMachine enemyBehaviorStateMachine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyBehaviorStateMachine = new EnemyBehaviorStateMachine(this, character);
        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.idleState);
    }

    // Update is called once per frame
    void Update()
    {
        rb.useGravity = !IsOnSlope();

        enemyBehaviorStateMachine.Update();
    }
    private void FixedUpdate()
    {
        enemyBehaviorStateMachine.PhysicsUpdate();
    }
    private void LateUpdate()
    {
        enemyBehaviorStateMachine.LateUpdate();
    }
    private void OnAnimationEnterEvent()
    {
        enemyBehaviorStateMachine.OnAnimationEnterEvent();
    }
    private void OnAnimationExitEvent()
    {
        enemyBehaviorStateMachine.OnAnimationExitEvent();
    }
    private void OnAnimationTransitionEvent()
    {
        enemyBehaviorStateMachine.OnAnimationTransitionEvent();
    }

    public bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.1f) == true)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
}
