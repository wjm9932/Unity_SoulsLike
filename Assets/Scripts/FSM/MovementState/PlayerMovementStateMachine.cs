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
    public Combo_1AttackState combo_1AttackState { get; }
    public Combo_2AttackState combo_2AttackState { get; }
    public Combo_3AttackState combo_3AttackState { get; }
    public LockOnWalkState lockOnWalkState { get; }


    public PlayerMovementStateMachine(Character character)
    {
        this.character = character;

        idleState = new IdleState(this);
        walkState = new WalkSate(this);
        sprintState = new SprintState(this);
        dodgeState = new DodgeState(this);

        combo_1AttackState = new Combo_1AttackState(this);
        combo_2AttackState = new Combo_2AttackState(this);
        combo_3AttackState = new Combo_3AttackState(this);
        lockOnWalkState = new LockOnWalkState(this);    
    }
}
