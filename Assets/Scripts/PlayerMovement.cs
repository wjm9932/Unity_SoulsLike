using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool isReadyToJump = true;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Mouse")]
    public float speedSmoothTime = 0.1f;
    public float turnSmoothTime = 0.1f;

    [Header("")]
    public Transform orientation;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private PlayerInput input;
    private Camera followCamera;
    private float turnSmoothVelocity;

    private MovementState state;
    private enum MovementState
    {
        walking,
        sprinting,
        air
    }
    // Start is called before the first frame update
    void Start()
    {
        followCamera = Camera.main;
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isReadyToJump);
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight* 0.5f + 0.2f, whatIsGround);

        StateHandler();

        if(input.jump == true && isGrounded == true && isReadyToJump == true)
        {
            Jump();
            isReadyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if(isGrounded == true)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
        SpeedControl();
    }
    public void Rotate()
    {
        var targetRotation = followCamera.transform.eulerAngles.y;
        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, 0f);

        transform.eulerAngles = Vector3.up * targetRotation;
    }

    private void StateHandler()
    {
        if(isGrounded == true && input.isSprinting == true)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if(isGrounded == true)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }
    }

    public void Move()
    {
        moveDirection = transform.forward * input.moveInput.y + transform.right * input.moveInput.x;

        if(IsOnSlope() == true && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);
            if(rb.velocity.y > 5)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if(isGrounded == true)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(isGrounded == false)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !IsOnSlope();
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
        exitingSlope = false;
    }

    private void SpeedControl()
    {
        if(IsOnSlope() == true && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private bool IsOnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f) == true)
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
}
