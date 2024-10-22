using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxHpState : PlayerMovementState
{
    private bool isDrinkFinished;
    public IncreaseMaxHpState(PlayerMovementStateMachine sm) : base(sm)
    {
    }
    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();

        //sm.owner.StartCoroutine(sm.owner.hpBar.ResizeStatusBarSize(10));
        //sm.owner.StartCoroutine(SetMaxHealth(10));

        //isDrinkFinished = false;
        //sm.owner.animator.SetBool("IsDrinkingPotion", true);
        //moveSpeed = sm.owner.walkSpeed;

        if (sm.owner.toBeUsedItem.UseItem(sm.owner) == true)
        {
            isDrinkFinished = false;
            sm.owner.animator.SetBool("IsDrinkingPotion", true);
            moveSpeed = sm.owner.walkSpeed;
            sm.owner.StartCoroutine(sm.owner.hpBar.ResizeStatusBarSize(sm.owner.toBeUsedItem.data.value));
            sm.owner.StartCoroutine(SetMaxHealth(sm.owner.toBeUsedItem.data.value));
        }
        else
        {
            TextManager.Instance.PlayNotificationText(TextManager.DisplayText.HP_IS_FULL);
            sm.ChangeState(sm.walkState);
        }
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

    private IEnumerator SetMaxHealth(float amount)
    {
        while(!sm.owner.hpBar.IsResizingDone())
        {
            yield return null;
        }
        sm.owner.maxHealth += amount;
    }
}
