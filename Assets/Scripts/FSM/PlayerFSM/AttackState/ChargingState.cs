using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingState : IState
{
    protected PlayerMovementStateMachine sm;

    private IEnumerator coroutineReference;
    private float accumulatedStamina;

    private float sizeDuration = 1.5f;
    private float sizeTimer;

    public ChargingState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }
    public virtual void Enter()
    {
        if(sm.owner.stamina <= 10f || sm.owner.playerQuestManager.isSkillOn == false)
        {
            sm.ChangeState(sm.walkState);
        }
        else
        {
            coroutineReference = PostSimulationUpdate();
            sm.owner.StartCoroutine(coroutineReference);
            sm.owner.rb.velocity = Vector3.zero;
            sm.owner.animator.SetBool("IsCharging", true); 
            accumulatedStamina = 0f;
            sizeTimer = 0f;

            var main = sm.owner.chargingEffect.main;
            main.startSize = 0f;
            sm.owner.chargingEffect.Play();
        }
    }
    public virtual void Update()
    {
        if (sm.owner.uiStateMachine.currentState is OpenState == true)
        {
            sm.ChangeState(sm.idleState);
        }

        UpdateStartSize();
        
        accumulatedStamina += Time.deltaTime * 10f;

        if (sm.owner.UseStamina(Time.deltaTime * 10f) == false || sm.owner.input.isChargingDone == true)
        {
            sm.ChangeState(sm.slashState);
        }
    }
    public virtual void PhysicsUpdate()
    {

    }
    public virtual void LateUpdate()
    {

    }
    public virtual void Exit()
    {
        if(coroutineReference != null)
        {
            sm.owner.SetDamage(Mathf.RoundToInt(accumulatedStamina * 1.5f));
            sm.owner.StopCoroutine(coroutineReference);
            sm.owner.animator.SetBool("IsCharging", false);
            sm.owner.chargingEffect.Stop();
        }
        

    }
    public virtual void OnAnimationEnterEvent()
    {

    }
    public virtual void OnAnimationExitEvent()
    {
    }
    public virtual void OnAnimationTransitionEvent()
    {

    }
    public virtual void OnAnimatorIK()
    {
        sm.owner.animator.SetFloat("HandWeight", 0);
    }
    private void Rotate()
    {
        Vector3 cameraForward = sm.owner.mainCamera.transform.forward;
        Vector3 flatForward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(flatForward);
        sm.owner.rb.MoveRotation(Quaternion.Slerp(sm.owner.rb.rotation, targetRotation, 25f * Time.fixedDeltaTime));
    }
    IEnumerator PostSimulationUpdate()
    {
        yield return null;
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            yield return waitForFixedUpdate;
            Rotate();
        }
    }

    private void UpdateStartSize()
    {
        if(sizeTimer >= sizeDuration)
        {
            return;
        }
        var main = sm.owner.chargingEffect.main;

        // 타이머 갱신
        sizeTimer += Time.deltaTime;


        // 현재 크기에서 목표 크기로 선형 보간
        float newSize = Mathf.Lerp(0f, 0.1f, sizeTimer / sizeDuration);

        // 업데이트된 크기 반영
        main.startSize = newSize;
        
    }
}