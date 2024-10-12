using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingState : EnemyPatternState
{
    private IEnumerator TrackTargetCoroutine;
    public TrackingState(EnemyBehaviorStateMachine sm) : base(sm)
    {

    }
    public override void Enter()
    {
        sm.owner.navMesh.isStopped = false;
        sm.owner.navMesh.speed = 2f;
        sm.owner.navMesh.stoppingDistance = 1f;
        TrackTargetCoroutine = TrackTarget();
        sm.owner.StartCoroutine(TrackTargetCoroutine);
        sm.owner.animator.SetFloat("Speed", sm.owner.navMesh.speed);
    }
    public override void Update()
    {
        base.Update();
    }
    public override void PhysicsUpdate()
    {

    }
    public override void LateUpdate()
    {

    }
    public override void Exit()
    {
        sm.owner.StopCoroutine(TrackTargetCoroutine);
    }
    public override void OnAnimationEnterEvent()
    {

    }
    public override void OnAnimationExitEvent()
    {

    }
    public override void OnAnimationTransitionEvent()
    {

    }
    public override void OnAnimatorIK()
    {

    }

    IEnumerator TrackTarget()
    {
        while (!sm.owner.isDead && sm.currentState == this)
        {
            sm.owner.navMesh.SetDestination(sm.owner.target.transform.position);


            yield return new WaitForSeconds(0.05f);

            if (sm.owner.navMesh.remainingDistance <= sm.owner.navMesh.stoppingDistance)
            {
                ChangeTargetState(sm.owner.entityType);
            }
            else if (sm.owner.navMesh.remainingDistance >= sm.owner.viewDistance)
            {
                sm.ChangeState(sm.patrolState);
            }
        }
    }
    private void ChangeTargetState(EntityType entityType)
    {
        switch (entityType)
        {
            case EntityType.ENEMY:
                sm.ChangeState(sm.swordAttackState);
                break;
            case EntityType.ARCHER:
                sm.ChangeState(sm.shootArrowState);
                break;
            default:
                Debug.Log("There is no type");
                break;
        }

    }
}
