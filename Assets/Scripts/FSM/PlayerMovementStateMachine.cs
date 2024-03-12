using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    public Character character { get;}
    public IdleState idleState { get; }
    public WalkSate walkState { get; }

    public PlayerMovementStateMachine(Character character)
    {
        this.character = character;

        idleState = new IdleState(this);
        walkState = new WalkSate(this);
    }
}
