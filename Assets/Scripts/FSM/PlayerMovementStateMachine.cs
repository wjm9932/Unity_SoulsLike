using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementStateMachine : StateMachine
{
    public Character character { get;}
    public IdleState idleState { get; }
    public WalkSate walkState { get; }
    public SprintState sprintState { get; }
    public DodgeState dodgeState { get; }

    public PlayerMovementStateMachine(Character character)
    {
        this.character = character;

        idleState = new IdleState(this);
        walkState = new WalkSate(this);
        sprintState = new SprintState(this);
        dodgeState = new DodgeState(this);
    }
}
