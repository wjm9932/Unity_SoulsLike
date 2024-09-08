using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkPotionState : PlayerMovementState
{
    public DrinkPotionState(PlayerMovementStateMachine sm) : base(sm)
    {

    }
    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();

        sm.character.animator.SetTrigger("DrinkPotion");
        moveSpeed = sm.character.walkSpeed;
    }

    // Update is called once per frame
    public override void Update()
    {
        SetMoveDirection();
        SpeedControl();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
