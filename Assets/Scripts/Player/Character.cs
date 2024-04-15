using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using UnityEngine.TextCore.Text;

public class Character : LivingEntity
{
    public CinemachineVirtualCamera lockOnCamera;
    public CinemachineFreeLook lockOffCamera;
    public Transform lockOnCameraPosition;
    public Transform camEyePos;
    public Transform leftHandPos;
    public TrailRenderer swordEffect;
    public LayerMask whatIsGround;
    public LayerMask enemy;
    public float walkSpeed;
    public float sprintSpeed;
    public Animator animator { get; private set; }
    public Camera followCamera { get; private set; }
    public Rigidbody rb { get; private set; }
    public float playerHeight { get; private set; }
    public PlayerInput input { get; private set; }
    public PlayerMovementStateMachine playerMovementStateMachine { get; private set; }
    
    [SerializeField]
    private float maxSlopeAngle;

    public Attack attack { get; private set; }
    public RaycastHit slopeHit;

    // Start is called before the first frame update
    private void Awake()
    {
        canBeDamaged = true;
        attack = GetComponent<Attack>();
        CameraStateMachine.Initialize(this);
        playerMovementStateMachine = new PlayerMovementStateMachine(this);
    }
    void Start()
    {
        followCamera = Camera.main;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerHeight = GetComponent<CapsuleCollider>().height;
        input = GetComponent<PlayerInput>();

        playerMovementStateMachine.ChangeState(playerMovementStateMachine.idleState);
        CameraStateMachine.Instance.ChangeState(CameraStateMachine.Instance.cameraLockOffState);
    }

    // Update is called once per frame
    void Update()
    {
        rb.useGravity = !IsOnSlope();
        playerMovementStateMachine.Update();
        CameraStateMachine.Instance.Update();

    }
    private void FixedUpdate()
    {
        playerMovementStateMachine.PhysicsUpdate();
    }
    private void LateUpdate()
    {
        playerMovementStateMachine.LateUpdate();
    }
    private void OnAnimationEnterEvent()
    {
        playerMovementStateMachine.OnAnimationEnterEvent();
    }
    private void OnAnimationExitEvent()
    {
        playerMovementStateMachine.OnAnimationExitEvent();
    }
    private void OnAnimationTransitionEvent()
    {
        playerMovementStateMachine.OnAnimationTransitionEvent();
    }
    private void OnAnimatorIK()
    {
        playerMovementStateMachine.OnAnimatorIK();
    }
    public Vector3 GetPlayerPosition()
    {
        return new Vector3(transform.position.x, transform.position.y + playerHeight / 2, transform.position.z);
    }

    public bool IsOnSlope()
    {
        if (Physics.Raycast(GetPlayerPosition(), Vector3.down, out slopeHit, playerHeight * 0.5f + 0.1f) == true)
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "EnemySword")
    //    {
    //        Attack enemy = other.transform.root.GetComponent<Attack>();

    //        if (enemy.canAttack == true && this.canBeDamaged == true)
    //        {
    //            animator.SetTrigger("Hit");
    //            playerMovementStateMachine.ChangeState(playerMovementStateMachine.hitState);

    //            var hitPoint = other.ClosestPoint(transform.position);
    //            Vector3 hitNormal = (transform.position - hitPoint).normalized;
    //            EffectManager.Instance.PlayHitEffect(hitPoint, hitNormal, other.transform, EffectManager.EffectType.Flesh);
    //        }
    //    }
    //}
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemySword")
        {
            if(playerMovementStateMachine.currentState != playerMovementStateMachine.hitState)
            {
                Attack enemy = other.transform.root.GetComponent<Attack>();

                if (enemy.canAttack == true && this.canBeDamaged == true)
                {
                    animator.SetTrigger("Hit");
                    playerMovementStateMachine.ChangeState(playerMovementStateMachine.hitState);

                    var hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = (transform.position - hitPoint).normalized;
                    EffectManager.Instance.PlayHitEffect(hitPoint, hitNormal, other.transform, EffectManager.EffectType.Flesh);
                }
            }
        }

    }
}
