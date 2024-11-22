using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingState : IState
{
    protected PlayerMovementStateMachine sm;

    private IEnumerator coroutineReference;
    private float accumulatedStamina;
    public ChargingState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }
    public virtual void Enter()
    {
        if(sm.owner.stamina <= 10f)
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
        }
    }
    public virtual void Update()
    {
        if (sm.owner.uiStateMachine.currentState is OpenState == true)
        {
            sm.ChangeState(sm.idleState);
        }
        
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
        sm.owner.SetDamage(Mathf.RoundToInt(accumulatedStamina * 1.5f));

        sm.owner.StopCoroutine(coroutineReference);
        sm.owner.animator.SetBool("IsCharging", false);
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
        sm.owner.rb.MoveRotation(Quaternion.Slerp(sm.owner.rb.rotation, targetRotation, 15f * Time.fixedDeltaTime));
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
}