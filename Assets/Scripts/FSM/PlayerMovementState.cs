using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementState : IState 
{
    protected PlayerMovementStateMachine sm;

    protected RaycastHit slopeHit;
    protected Vector3 moveDirection;
    protected Vector3 lookAtDirection;

    protected float maxSlopeAngle = 45f;
    protected float moveSpeed;
    protected bool isGrounded;

    public PlayerMovementState(PlayerMovementStateMachine sm)
    {
        this.sm = sm;
    }

    public virtual void Enter()
    {
    }
    public virtual void Update()
    {
        isGrounded = Physics.Raycast(GetPlayerPosition(), Vector3.down, sm.character.playerHeight * 0.5f + 0.2f, sm.character.whatIsGround);

        SetMoveDirection();
        sm.character.rb.useGravity = !IsOnSlope();
    }
    public virtual void PhysicsUpdate()
    {
        Move();
        SpeedControl();
    }
    public virtual void LateUpdate()
    {
        Rotate();
    }
    public virtual void Exit()
    {
        sm.character.rb.velocity = Vector3.zero;
    }

    private void Move()
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

    private void SpeedControl()
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
            sm.character.transform.rotation = (Quaternion.Slerp(sm.character.transform.rotation, targetRotation, 15f * Time.deltaTime));
        }
    }

    Vector3 GetPlayerPosition()
    {
        return new Vector3(sm.character.transform.position.x, sm.character.transform.position.y + sm.character.playerHeight / 2, sm.character.transform.position.z);
    }

    private bool IsOnSlope()
    {
        if (Physics.Raycast(GetPlayerPosition(), Vector3.down, out slopeHit, sm.character.playerHeight * 0.5f + 0.3f) == true)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void SetMoveDirection()
    {
        Vector3 forward = sm.character.followCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        moveDirection = forward * sm.character.input.moveInput.y + sm.character.followCamera.transform.right * sm.character.input.moveInput.x;
        lookAtDirection = forward * sm.character.input.rotationInput.y + sm.character.followCamera.transform.right * sm.character.input.rotationInput.x;
    }
}
