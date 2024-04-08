using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public NavMeshAgent navMesh { get; private set; }
    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }

    [SerializeField]
    private Character character;
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
        //navMesh.updatePosition = false;
        enemyBehaviorStateMachine = new EnemyBehaviorStateMachine(this, character);
        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.idleState);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        //}
        //navMesh.nextPosition = transform.position;

        //transform.position = Vector3.MoveTowards(transform.position, character.transform.position, Time.deltaTime * 1f);
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
}
