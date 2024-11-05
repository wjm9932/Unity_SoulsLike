using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState
{
    protected EnemyBehaviorStateMachine sm;
    public DieState(EnemyBehaviorStateMachine sm)
    {
        this.sm = sm;
    }

    public virtual void Enter()
    {
        sm.owner.canAttack = false;
        SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ENEMY_DIE, 0.2f);
        sm.owner.animator.SetTrigger("Die");
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
