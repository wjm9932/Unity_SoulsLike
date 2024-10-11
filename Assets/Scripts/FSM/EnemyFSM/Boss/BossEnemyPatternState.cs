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
        Vector3 dir = sm.character.transform.position - sm.owner.transform.position;
        dir.y = 0;

        return Quaternion.LookRotation(dir);
    }
}
