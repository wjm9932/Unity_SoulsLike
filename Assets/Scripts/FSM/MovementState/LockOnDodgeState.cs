using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnDodgeState : PlayerMovementState
{
    private bool isDodgeFinished;
    private IEnumerator coroutineReference;


    private Vector3 target;

    public LockOnDodgeState(PlayerMovementStateMachine sm) : base(sm)
    {

    }
    public override void Enter()
    {
        target = new Vector3(0.18f, 1.57f, 10.11f);

        coroutineReference = PostSimulationUpdate();
        sm.character.StartCoroutine(coroutineReference);

        isDodgeFinished = false;
        sm.character.rb.velocity = Vector3.zero;

        sm.character.animator.SetFloat("Horizontal", sm.character.input.moveInput.x);
        sm.character.animator.SetFloat("Vertical", sm.character.input.moveInput.y);
        sm.character.animator.SetTrigger("IsRolling");
        Dodge();
    }
    public override void Update()
    {
        sm.character.rb.useGravity = !IsOnSlope();

        if (isDodgeFinished == true)
        {
            if(CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOnState)
            {
                sm.ChangeState(sm.lockOnWalkState);
            }
            else
            {
                sm.ChangeState(sm.walkState);
            }
        }
    }
    public override void PhysicsUpdate()
    {

    }
    public override void LateUpdate()
    {

    }
    public override void Exit()
    {
        sm.character.StopCoroutine(coroutineReference);
    }
    public override void OnAnimationEnterEvent()
    {

    }
    public override void OnAnimationExitEvent()
    {
        isDodgeFinished = true;
    }
    public override void OnAnimationTransitionEvent()
    {

    }
    private void Dodge()
    {
        if (IsOnSlope() == true)
        {
            sm.character.rb.AddForce(GetSlopeMoveDirection() * 7f, ForceMode.Impulse);
            if (sm.character.rb.velocity.y > 5)
            {
                sm.character.rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else
        {
            Vector3 moveDir = sm.character.transform.forward * sm.character.input.moveInput.y + sm.character.transform.right * sm.character.input.moveInput.x;
            sm.character.rb.AddForce(moveDir * 7f, ForceMode.Impulse);
        }
    }

    protected override Vector3 GetSlopeMoveDirection()
    {
        Vector3 moveDir = sm.character.transform.forward * sm.character.input.moveInput.y + sm.character.transform.right * sm.character.input.moveInput.x;
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

    private void Rotate()
    {
        Vector3 direction = new Vector3(target.x - sm.character.rb.position.x, 0, target.z - sm.character.rb.position.z);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        sm.character.rb.MoveRotation(Quaternion.Slerp(sm.character.rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
    }

    IEnumerator PostSimulationUpdate()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            yield return waitForFixedUpdate;
            Rotate();
        }
    }
}
