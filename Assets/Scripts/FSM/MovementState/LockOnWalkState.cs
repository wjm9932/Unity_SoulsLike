using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LockOnWalkState : PlayerMovementState
{
    private Vector3 target;
    private IEnumerator coroutineReference;
    public LockOnWalkState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        target = new Vector3(0.18f, 1.57f, 10.11f);

        coroutineReference = PostSimulationUpdate();
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
        base.PhysicsUpdate();

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
