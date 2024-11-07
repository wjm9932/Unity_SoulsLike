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
        SetMoveDirection();
        coroutineReference = PostSimulationUpdate();
        sm.owner.StartCoroutine(coroutineReference);
    }
    public virtual void Update()
    {
        SetMoveDirection();
        SpeedControl();

        if (sm.owner.input.moveInput == Vector2.zero)
        {
            sm.ChangeState(sm.idleState);
        }

        if (sm.owner.input.IsClickItemInInventory(sm.owner.OnClickItem) == true)
        {
            sm.ChangeState(sm.owner.toBeUsedItem.GetTargetState(sm));
        }

        if (sm.owner.input.isUsingQuickSlot == true)
        {
            if (sm.owner.quickSlot != null)
            {
                sm.ChangeState(sm.owner.quickSlot.GetTargetState(sm));
            }
        }
        if (sm.owner.uiStateMachine.currentState is InteractStateUI == true)
        {
            sm.ChangeState(sm.interactState);
        }

        if (sm.owner.uiStateMachine.currentState is OpenState == false)
        {
            if (sm.owner.input.isDodging == true)
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
            if (sm.owner.input.isAttack == true)
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
        sm.owner.StopCoroutine(coroutineReference);
    }

    protected virtual void Move()
    {
        if (sm.owner.IsOnSlope() == true)
        {
            sm.owner.rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            sm.owner.rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
    }
    protected void SpeedControl()
    {
        if (sm.owner.IsOnSlope() == true)
        {
            if (sm.owner.rb.velocity.magnitude > moveSpeed)
            {
                sm.owner.rb.velocity = sm.owner.rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(sm.owner.rb.velocity.x, 0f, sm.owner.rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                sm.owner.rb.velocity = new Vector3(limitedVel.x, sm.owner.rb.velocity.y, limitedVel.z);
            }
        }
    }



    private void Rotate()
    {
        if (sm.owner.rb.velocity.magnitude > 0.2f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookAtDirection);
            sm.owner.rb.MoveRotation(Quaternion.Slerp(sm.owner.rb.rotation, targetRotation, 15f * Time.fixedDeltaTime));
        }
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
    protected virtual Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, sm.owner.slopeHit.normal).normalized;
    }
    protected void SetMoveDirection()
    {
        Vector3 forward = sm.owner.mainCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        moveDirection = forward * sm.owner.input.moveInput.y + sm.owner.mainCamera.transform.right * sm.owner.input.moveInput.x;
        lookAtDirection = forward * sm.owner.input.rotationInput.y + sm.owner.mainCamera.transform.right * sm.owner.input.rotationInput.x;
    }
    void UpdateAnimation()
    {
        sm.owner.animator.SetFloat("Speed", sm.owner.rb.velocity.magnitude, 0.08f, Time.deltaTime);
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
}
