using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UX;
public abstract class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine sm;

    protected Vector3 moveDirection;
    protected Vector3 lookAtDirection;

    protected float moveSpeed;

    private IEnumerator coroutineReference;

    public PlayerMovementState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }

    public virtual void Enter()
    {
        coroutineReference = PostSimulationUpdate();
        sm.character.StartCoroutine(coroutineReference);
    }
    public virtual void Update()
    {
        SetMoveDirection();
        SpeedControl();

        if (sm.character.input.moveInput == Vector2.zero)
        {
            sm.ChangeState(sm.idleState);
        }
        
        if (sm.character.input.IsClickItemInInventory(sm.character.OnClickItem) == true)
        {
            sm.ChangeState(sm.drinkPotionState);
        }
        
        if (sm.uiStatMachine.currentState is OpenState == false)
        {
            if (sm.character.input.isDodging == true)
            {
                if (CameraStateMachine.Instance.currentState == CameraStateMachine.Instance.cameraLockOnState)
                {
                    sm.ChangeState(sm.lockOnDodgeState);
                }
                else
                {
                    sm.ChangeState(sm.dodgeState);
                }
            }
            if (sm.character.input.isAttack == true)
            {
                sm.ChangeState(sm.combo_1AttackState);
            }
        }
    }
    public virtual void PhysicsUpdate()
    {
        Move();
    }
    public virtual void LateUpdate()
    {
        UpdateAnimation();
    }
    public virtual void Exit()
    {
        sm.character.StopCoroutine(coroutineReference);
    }

    protected virtual void Move()
    {
        if (sm.character.IsOnSlope() == true)
        {
            sm.character.rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            sm.character.rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
    }
    protected void SpeedControl()
    {
        if (sm.character.IsOnSlope() == true)
        {
            if (sm.character.rb.velocity.magnitude > moveSpeed)
            {
                sm.character.rb.velocity = sm.character.rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(sm.character.rb.velocity.x, 0f, sm.character.rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                sm.character.rb.velocity = new Vector3(limitedVel.x, sm.character.rb.velocity.y, limitedVel.z);
            }
        }
    }
    private void Rotate()
    {
        if (sm.character.rb.velocity.magnitude > 0.2f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookAtDirection);
            sm.character.rb.MoveRotation(Quaternion.Slerp(sm.character.rb.rotation, targetRotation, 15f * Time.fixedDeltaTime));
        }
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
    protected virtual Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, sm.character.slopeHit.normal).normalized;
    }
    protected void SetMoveDirection()
    {
        Vector3 forward = sm.character.mainCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        moveDirection = forward * sm.character.input.moveInput.y + sm.character.mainCamera.transform.right * sm.character.input.moveInput.x;
        lookAtDirection = forward * sm.character.input.rotationInput.y + sm.character.mainCamera.transform.right * sm.character.input.rotationInput.x;
    }
    void UpdateAnimation()
    {
        sm.character.animator.SetFloat("Speed", sm.character.rb.velocity.magnitude, 0.08f, Time.deltaTime);
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
        sm.character.animator.SetFloat("HandWeight", 0);
    }
}
