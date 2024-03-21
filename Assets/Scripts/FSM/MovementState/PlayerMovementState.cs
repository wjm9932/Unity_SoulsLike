using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class PlayerMovementState : IState 
{
    protected PlayerMovementStateMachine sm;

    protected RaycastHit slopeHit;
    protected Vector3 moveDirection;
    protected Vector3 lookAtDirection;

    protected float maxSlopeAngle;
    protected float moveSpeed;
    protected bool isGrounded;

    private IEnumerator coroutineReference;

    public PlayerMovementState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
        maxSlopeAngle = 45f;
    }

    public virtual void Enter()
    {
        coroutineReference = PostSimulationUpdate();
        sm.character.StartCoroutine(coroutineReference);
    }
    public virtual void Update()
    {
        isGrounded = Physics.Raycast(GetPlayerPosition(), Vector3.down, sm.character.playerHeight * 0.5f + 0.2f, sm.character.whatIsGround);

        sm.character.rb.useGravity = !IsOnSlope();

        SetMoveDirection();

        if (sm.character.input.moveInput == Vector2.zero)
        {
            sm.ChangeState(sm.idleState);
        }
        if(sm.character.input.isDodging == true)
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
    public virtual void PhysicsUpdate()
    {
        Move();
        SpeedControl();
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

    protected void SpeedControl()
    {
        if (IsOnSlope() == true)
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
            sm.character.rb.MoveRotation(Quaternion.Slerp(sm.character.rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
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

    Vector3 GetPlayerPosition()
    {
        return new Vector3(sm.character.transform.position.x, sm.character.transform.position.y + sm.character.playerHeight / 2, sm.character.transform.position.z);
    }

    protected bool IsOnSlope()
    {
        if (Physics.Raycast(GetPlayerPosition(), Vector3.down, out slopeHit, sm.character.playerHeight * 0.5f + 0.3f) == true)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    protected virtual Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(sm.character.transform.forward, slopeHit.normal).normalized;
    }

    protected void SetMoveDirection()
    {
        Vector3 forward = sm.character.followCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        moveDirection = forward * sm.character.input.moveInput.y + sm.character.followCamera.transform.right * sm.character.input.moveInput.x;
        lookAtDirection = forward * sm.character.input.rotationInput.y + sm.character.followCamera.transform.right * sm.character.input.rotationInput.x;
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
}
