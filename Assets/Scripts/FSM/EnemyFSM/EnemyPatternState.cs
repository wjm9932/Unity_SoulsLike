using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyPatternState : IState
{
    protected EnemyBehaviorStateMachine sm;
    protected float stoppingDistance;
    protected float agentSpeed;
    public EnemyPatternState(EnemyBehaviorStateMachine sm)
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
}
