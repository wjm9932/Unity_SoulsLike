using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrinkPotionState : PlayerMovementState
{
    private bool isDrinkFinished;
    private IEnumerator recoverCoroutine;
    public DrinkPotionState(PlayerMovementStateMachine sm) : base(sm)
    {
    }
    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();
        recoverCoroutine = RecoverHPOverTime(0.5f, sm.owner.toBeUsedItem.data.value);

        if (sm.owner.toBeUsedItem.UseItem(sm.owner) == true)
        {
            isDrinkFinished = false;
            sm.owner.animator.SetBool("IsDrinkingPotion", true);
            moveSpeed = sm.owner.walkSpeed;

            sm.owner.StartCoroutine(recoverCoroutine);
        }
        else
        {
            TextManager.Instance.PlayNotificationText(TextManager.DisplayText.HP_IS_FULL);
            SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ALERT, 0.2f);
            sm.ChangeState(sm.walkState);
        }
    }

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
        sm.owner.animator.SetBool("IsDrinkingPotion", false);
        sm.owner.StopCoroutine(recoverCoroutine);
    }
    public override void OnAnimationTransitionEvent()
    {
        SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.DRINK, 0.7f);
    }

    public override void OnAnimationExitEvent()
    {
        isDrinkFinished = true;
    }

    private IEnumerator RecoverHPOverTime(float duration, float totalRecovery)
    {
        float elapsedTime = 0f;
        float amountPerTick = totalRecovery / 100f; 
        float interval = duration / 100f; 

        while (elapsedTime < duration)
        {
            sm.owner.RecoverHP(amountPerTick);

            elapsedTime += interval;
            yield return new WaitForSeconds(interval);
        }
    }
}
