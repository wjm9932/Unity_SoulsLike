using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatternState : IState
{
    protected EnemyBehaviorStateMachine sm;
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

    protected Quaternion GetMoveRotationAngle()
    {
        Vector3 direction = sm.owner.navMesh.velocity;
        direction.y = 0;

        return Quaternion.LookRotation(direction);
    }
}
