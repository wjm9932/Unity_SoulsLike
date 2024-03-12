using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;
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
    public LayerMask whatIsGround;
    private float playerHeight;
    private bool isGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Mouse")]
    public float speedSmoothTime = 0.1f;
    public float turnSmoothTime = 0.1f;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private PlayerInput input;
    private Camera followCamera;
    private Animator animatorPlayer;
    private float turnSmoothVelocity;

    private PlayerState playerState;
    private bool isRotating = false;

    void Awake()
    {
        StartCoroutine(PostSimulationUpdate());
    }

    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerState>();
        followCamera = Camera.main;
        input = GetComponent<PlayerInput>();
        animatorPlayer = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerHeight = GetComponent<CapsuleCollider>().height;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rb.velocity.magnitude);
        isGrounded = Physics.Raycast(GetPlayerPosition(), Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (input.isJumping == true && isGrounded == true && isReadyToJump == true)
        {
            Jump();
            isReadyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (input.isDodging == true && (playerState.state == PlayerState.State.Idle || playerState.state == PlayerState.State.Sprinting))
        {
            Dodge();
        }

        if(playerState.state != PlayerState.State.Dodging && playerState.state != PlayerState.State.Attacking)
        {
            StateHandler();
        }
        UpdateAnimation(input);
        rb.useGravity = !IsOnSlope();
    }
    private void FixedUpdate()
    {
        SetMoveDirection();

        if (playerState.state == PlayerState.State.Idle || playerState.state == PlayerState.State.Sprinting)
        {
            Move();
            SpeedControl();
        }
        else if (playerState.state == PlayerState.State.Attacking)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }
    public void Rotate()
    {
        if (rb.velocity.magnitude > 0.2f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
    }

    IEnumerator PostSimulationUpdate()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            yield return waitForFixedUpdate;
            if (playerState.state != PlayerState.State.Dodging)
            {
                Rotate();
            }
        }
    }

    IEnumerator RotateBack()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
        isRotating = true;

        float targetAngle = transform.eulerAngles.y + 180f;
        float currentAngle = transform.eulerAngles.y;

        while (Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) > 0.5f)
        {
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, 500f * Time.deltaTime);
            rb.MoveRotation(Quaternion.Euler(0f, currentAngle, 0f));
            yield return waitForFixedUpdate;
        }

        isRotating = false;
    }

    private void Dodge()
    {
        transform.LookAt(transform.position + moveDirection);

        rb.velocity = Vector3.zero;
        if (IsOnSlope() == true && !exitingSlope)
        {

            rb.AddForce(GetSlopeMoveDirection() * 10f, ForceMode.Impulse);
            if (rb.velocity.y > 5)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else
        {
            rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
        }
    }
    private void StateHandler()
    {
        if (isGrounded == true && input.isSprinting == true)
        {
            playerState.state = PlayerState.State.Sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (isGrounded == true)
        {
            playerState.state = PlayerState.State.Idle;
            moveSpeed = walkSpeed;
        }
        else
        {
            playerState.state = PlayerState.State.Jumping;
        }
    }

    public void Move()
    {
        if (IsOnSlope() == true && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);
            if (rb.velocity.y > 5)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (isGrounded == true && input.isJumping == false)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (isGrounded == false)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
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
        if (IsOnSlope() == true && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
            if (input.moveInput == Vector2.zero)
            {
                rb.velocity = Vector3.zero;
            }
        }
        else if (input.moveInput.magnitude < 0.2f && input.isJumping == false)
        {
            if (isGrounded == true)
            {
                rb.velocity = Vector3.zero;
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
        if (Physics.Raycast(GetPlayerPosition(), Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f) == true)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    Vector3 GetPlayerPosition()
    {
        return new Vector3(transform.position.x, transform.position.y + playerHeight / 2, transform.position.z);
    }

    void UpdateAnimation(PlayerInput input)
    {
        animatorPlayer.SetFloat("Speed", rb.velocity.magnitude, 0.08f, Time.deltaTime);

        if (input.isDodging == true && (playerState.state == PlayerState.State.Idle || playerState.state == PlayerState.State.Sprinting))
        {
            animatorPlayer.SetTrigger("IsRolling");
            playerState.state = PlayerState.State.Dodging;
        }
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void SetMoveDirection()
    {
        Vector3 forward = followCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();

        moveDirection = forward * input.moveInput.y + followCamera.transform.right * input.moveInput.x;
    }
}
