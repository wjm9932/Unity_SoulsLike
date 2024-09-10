using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkPotionState : PlayerMovementState
{
    private bool isDrinkFinished;
    public DrinkPotionState(PlayerMovementStateMachine sm) : base(sm)
    {

    }
    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();
        if (sm.owner.clickedItem.UseItem(sm.owner) == true)
        {
            isDrinkFinished = false;
            sm.owner.animator.SetTrigger("DrinkPotion");
            moveSpeed = sm.owner.walkSpeed;
        }
        else
        {
            sm.ChangeState(sm.walkState);
        }
        
    }

    // Update is called once per frame
    public override void Update()
    {
        SetMoveDirection();
        SpeedControl();

        if(moveDirection == Vector3.zero)
        {
            sm.owner.rb.velocity = Vector3.zero;
        }

        if(isDrinkFinished == true)
        {
            sm.ChangeState(sm.walkState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void OnAnimationExitEvent()
    {
        isDrinkFinished = true;
    }
}
