using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementState : IState 
{
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
}
