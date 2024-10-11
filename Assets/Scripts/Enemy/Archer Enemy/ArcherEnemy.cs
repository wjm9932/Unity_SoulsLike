using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : Enemy
{
    public GameObject arrow;
    public Transform arrowPosition;
    private EnemyBehaviorStateMachine enemyBehaviorStateMachine;
    protected override void Awake()
    {
        base.Awake();
        navMesh.updateRotation = false;
        entityType = EntityType.ARCHER;
    }
    // Start is called before the first frame update
    void Start()
    {
        health = 10f;
        fieldOfView = 50f;
        viewDistance = 10f;
        enemyBehaviorStateMachine = new EnemyBehaviorStateMachine(this);
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

    public override void Die()
    {
        base.Die();

        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.dieState);
    }
}
