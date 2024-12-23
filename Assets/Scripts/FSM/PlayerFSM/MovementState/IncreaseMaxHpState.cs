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

        if (sm.owner.toBeUsedItem.UseItem(sm.owner) == true)
        {
            isDrinkFinished = false;
            sm.owner.animator.SetBool("IsIncreaseHp", true);
            moveSpeed = sm.owner.walkSpeed;
            sm.owner.StartCoroutine(sm.owner.hpBar.ResizeStatusBarSize(sm.owner.toBeUsedItem.data.value));
            sm.owner.StartCoroutine(SetMaxHealth(sm.owner.toBeUsedItem.data.value));
            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.INCREASE_MAX_HP, 0.5f);
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
        sm.owner.animator.SetBool("IsIncreaseHp", false);
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
        yield return null;
        sm.owner.maxHealth += amount;
    }
}
