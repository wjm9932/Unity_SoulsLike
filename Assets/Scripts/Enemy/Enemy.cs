using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    [SerializeField]
    private Character character;

    public NavMeshAgent navMesh { get; private set; }
    private EnemyBehaviorStateMachine enemyBehaviorStateMachine;
    private void Awake()
    {
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
