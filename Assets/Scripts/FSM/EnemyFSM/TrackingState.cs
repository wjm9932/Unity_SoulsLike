using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        sm.owner.navMesh.stoppingDistance = sm.owner.trackingStopDistance;

        TrackTargetCoroutine = TrackTarget();
        sm.owner.StartCoroutine(TrackTargetCoroutine);

        sm.owner.animator.SetFloat("Speed", sm.owner.navMesh.speed);
        sm.owner.navMesh.avoidancePriority = 51;
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
            else if (sm.owner.navMesh.remainingDistance >= sm.owner.viewDistance || IsPlayerOnNavMesh() == false)
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

    bool IsPlayerOnNavMesh()
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(sm.owner.target.transform.position, out hit, 1.0f, sm.owner.navMesh.areaMask);
    }
}
