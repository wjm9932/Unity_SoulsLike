using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockOnWalkState : PlayerMovementState
{
    private IEnumerator coroutineReference;
    public LockOnWalkState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        coroutineReference = PostSimulationUpdate();
    }

    public override void Enter()
    {
        sm.character.StartCoroutine(coroutineReference);
        moveSpeed = 5f;
    }

    public override void Update()
    {
        base.Update();

        if (CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOffState)
        {
            sm.ChangeState(sm.walkState);
        }
    }

    public override void PhysicsUpdate()
    {
        Move();
        SpeedControl();
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
        
    }
    public override void Exit()
    {
        sm.character.StopCoroutine(coroutineReference);
    }
    private void Rotate()
    {
        Vector3 direction = new Vector3(CameraStateMachine.Instance.cameraLockOnState.target.position.x - sm.character.rb.position.x, 0, CameraStateMachine.Instance.cameraLockOnState.target.position.z - sm.character.rb.position.z);

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        sm.character.rb.MoveRotation(Quaternion.Slerp(sm.character.rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
    }
    protected override void Move()
    {
        if (IsOnSlope() == true)
        {
            sm.character.rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);
            if (sm.character.rb.velocity.y > 5)
            {
                sm.character.rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else
        {
            sm.character.rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
    }
    protected override Vector3 GetSlopeMoveDirection()
    {
        Vector3 moveDir = sm.character.transform.forward * sm.character.input.moveInput.y + sm.character.transform.right * sm.character.input.moveInput.x;
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
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
