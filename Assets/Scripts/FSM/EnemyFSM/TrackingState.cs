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
        sm.owner.navMesh.speed = sm.owner.trackingSpeed;
        sm.owner.navMesh.stoppingDistance = sm.owner.trackingStopDistance;

        TrackTargetCoroutine = TrackTarget();
        sm.owner.StartCoroutine(TrackTargetCoroutine);

        sm.owner.animator.SetFloat("Speed", sm.owner.navMesh.speed);
        sm.owner.navMesh.avoidancePriority = 51;
    }
    public override void Update()
    {
        if (sm.owner.target.GetComponent<LivingEntity>().isDead == true)
        {
            sm.ChangeState(sm.patrolState);
        }
        else
        {
            base.Update();
        }
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

            if (sm.owner.navMesh.remainingDistance >= sm.owner.viewDistance || IsPlayerOnNavMesh() == false)
            {
                sm.ChangeState(sm.patrolState);
            }
            else if (sm.owner.navMesh.remainingDistance <= sm.owner.navMesh.stoppingDistance)
            {
                ChangeTargetState(sm.owner.entityType);
            }
        }
    }
    private void ChangeTargetState(EntityType entityType)
    {
        switch (entityType)
        {
            case EntityType.WARRIOR:
                sm.ChangeState(sm.swordAttackState);
                break;
            case EntityType.ARCHER:
                sm.ChangeState(sm.shootArrowState);
                break;
            case EntityType.TANK:
                sm.ChangeState(sm.hammerAttackState);
                break;
            default:
                Debug.Log("There is no entitiy type");
                break;
        }

    }

    bool IsPlayerOnNavMesh()
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(sm.owner.target.transform.position, out hit, 0.1f, sm.owner.navMesh.areaMask);
    }
}
