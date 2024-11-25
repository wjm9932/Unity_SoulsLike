using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossEnemyPatternState : IState
{
    protected BossEnemyBehaviorStateMachine sm;
    protected float stoppingDistance;
    protected float agentSpeed;
    protected Quaternion dir;
    public BossEnemyPatternState(BossEnemyBehaviorStateMachine sm)
    {
        this.sm = sm;
    }

    public virtual void Enter()
    {
        
    }
    public virtual void Update()
    {
       if(sm.owner.target.GetComponent<LivingEntity>().isDead == true)
        {
            sm.ChangeState(sm.patrolState);
        }
    }
    public virtual void PhysicsUpdate()
    {

    }
    public virtual void LateUpdate()
    {

    }
    public virtual void Exit()
    {

    }
    public virtual void OnAnimationEnterEvent()
    {

    }
    public virtual void OnAnimationExitEvent()
    {

    }
    public virtual void OnAnimationTransitionEvent()
    {

    }
    public virtual void OnAnimatorIK()
    {

    }
    protected virtual Quaternion GetLookAtAngle()
    {
        Vector3 dir = sm.owner.target.transform.position - sm.owner.transform.position;
        dir.y = 0;

        return Quaternion.LookRotation(dir);
    }
}
