using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrinkArmorBuffPotionState : PlayerMovementState
{
    private bool isDrinkFinished;
    public DrinkArmorBuffPotionState(PlayerMovementStateMachine sm) : base(sm)
    {
    }

    public override void Enter()
    {
        base.Enter();

        isDrinkFinished = false;
        sm.owner.animator.SetBool("IsDrinkingPotion", true);
        moveSpeed = sm.owner.walkSpeed;
    }

    public override void Update()
    {
        SetMoveDirection();
        SpeedControl();

        if (moveDirection == Vector3.zero)
        {
            sm.owner.rb.velocity = Vector3.zero;
        }

        if (isDrinkFinished == true)
        {
            sm.owner.toBeUsedItem.UseItem(sm.owner);
            sm.owner.playerBuff.AddBuff(BuffManager.BuffType.ARMOR, sm.owner.toBeUsedItem.data.value);
            sm.ChangeState(sm.walkState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        sm.owner.animator.SetBool("IsDrinkingPotion", false);
    }

    public override void OnAnimationExitEvent()
    {
        isDrinkFinished = true;
    }
}
