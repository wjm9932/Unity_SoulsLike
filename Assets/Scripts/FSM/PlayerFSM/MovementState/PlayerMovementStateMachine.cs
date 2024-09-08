using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerFSM;

public class PlayerMovementStateMachine : StateMachine
{
    public UIStateMachine uiStatMachine { get; }
    public Character character { get;}
    public IdleState idleState { get; }
    public WalkSate walkState { get; }
    public SprintState sprintState { get; }
    public DodgeState dodgeState { get; }
    public Combo_1AttackState combo_1AttackState { get; }
    public Combo_2AttackState combo_2AttackState { get; }
    public Combo_3AttackState combo_3AttackState { get; }
    public LockOnWalkState lockOnWalkState { get; }
    public LockOnDodgeState lockOnDodgeState { get; }
    public HitState hitState { get; }
    public DrinkPotionState drinkPotionState { get; }


    public PlayerMovementStateMachine(Character character, UIStateMachine uiStateMachine)
    {
        this.character = character;
        this.uiStatMachine = uiStateMachine;
        idleState = new IdleState(this);
        walkState = new WalkSate(this);
        sprintState = new SprintState(this);
        dodgeState = new DodgeState(this);
        drinkPotionState = new DrinkPotionState(this);

        combo_1AttackState = new Combo_1AttackState(this);
        combo_2AttackState = new Combo_2AttackState(this);
        combo_3AttackState = new Combo_3AttackState(this);
        lockOnWalkState = new LockOnWalkState(this);
        lockOnDodgeState = new LockOnDodgeState(this);
        hitState = new HitState(this);  
    }
}
