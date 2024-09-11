using System.Collections;
using System.Collections.Generic;
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

        sm.owner.animator.SetLayerWeight(1, 1);
        recoverCoroutine = RecoverHPOverTime(0.5f, sm.owner.clickedItem.data.value);

        if (sm.owner.clickedItem.UseItem(sm.owner) == true)
        {
            isDrinkFinished = false;
            sm.owner.animator.SetTrigger("DrinkPotion");
            moveSpeed = sm.owner.walkSpeed;

            sm.owner.StartCoroutine(recoverCoroutine);
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
        sm.owner.StopCoroutine(recoverCoroutine);
        sm.owner.animator.SetLayerWeight(1, 0);
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
