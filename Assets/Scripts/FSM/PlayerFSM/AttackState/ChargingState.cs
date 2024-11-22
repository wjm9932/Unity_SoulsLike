using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingState : IState
{
    protected PlayerMovementStateMachine sm;

    private IEnumerator coroutineReference;
    public ChargingState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }
    public virtual void Enter()
    {
        coroutineReference = PostSimulationUpdate();
        sm.owner.StartCoroutine(coroutineReference);

        sm.owner.animator.SetBool("IsCharging", true);
    }
    public virtual void Update()
    {
        sm.owner.staminaRecoverCoolTime = Character.targetStaminaRecoverCoolTime;
        sm.owner.stamina -= Time.deltaTime * 10f;
        if (sm.owner.uiStateMachine.currentState is OpenState == true)
        {
            sm.ChangeState(sm.idleState);
        }

        if (sm.owner.stamina <= 0f || sm.owner.input.isChargingDone == true)
        {
            sm.ChangeState(sm.idleState);
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
        sm.owner.animator.SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
        sm.owner.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, sm.owner.animator.GetFloat("HandWeight"));
        sm.owner.animator.SetIKPosition(AvatarIKGoal.LeftHand, sm.owner.leftHandPos.position);
    }
    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(sm.owner.mainCamera.transform.forward);
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