using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool isReadyToJump;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool isGrounded;

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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight* 0.5f + 0.2f, whatIsGround);

        if(input.jump == true && isGrounded == true)
        {
            Jump();
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
        SpeedControl();
        Rotate();
        Move();
    }
    public void Rotate()
    {
        var targetRotation = followCamera.transform.eulerAngles.y;
        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, 0f);

        transform.eulerAngles = Vector3.up * targetRotation;
    }

    public void Move()
    {
        moveDirection = transform.forward * input.moveInput.y + transform.right * input.moveInput.x;
        if(isGrounded == true)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(isGrounded == false)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
