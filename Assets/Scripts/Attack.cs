using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attack : MonoBehaviour
{
    public Transform leftHandPos;

    private PlayerInput input;
    private PlayerState playerState;
    private Animator animator;
    private Camera followCamera;
    private ComboAttack currentCombo;
    private bool canComboAttack = true;
    private Rigidbody rb;
    enum ComboAttack
    {
        Idle,
        Attack1,
        Attack2,
        Attack3,
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerState>();
        followCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(input.isAttack == true)
        {
            SwordAttack();
        }
        
        ResetPlayerState();
    }

    void ResetPlayerState()
    {
        if(playerState.state == PlayerState.State.Attacking)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") == true)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f && animator.IsInTransition(0) == false)
                {
                    animator.SetTrigger("ResetAttackCombo");
                    playerState.state = PlayerState.State.Idle;
                    currentCombo = ComboAttack.Idle;
                    canComboAttack = true;
                }
            }
        }
    }

    void SetCanComboAttackTrue()
    {
        canComboAttack = true;
    }
    void SwordAttack()
    {
        if(canComboAttack == true && playerState.state != PlayerState.State.Dodging)
        {
            Vector3 forward = followCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 moveDirection = forward;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection); 
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 50f * Time.fixedDeltaTime));

            playerState.state = PlayerState.State.Attacking;

            if (currentCombo == ComboAttack.Idle)
            {
                animator.SetTrigger("IsAttack1");
                currentCombo = ComboAttack.Attack1;
                canComboAttack = false;
            }
            else if(currentCombo == ComboAttack.Attack1)
            {
                animator.SetTrigger("IsAttack2");
                currentCombo = ComboAttack.Attack2;
                canComboAttack = false;
            }
            else if(currentCombo == ComboAttack.Attack2)
            {
                animator.SetTrigger("IsAttack3");
                currentCombo = ComboAttack.Attack3;
                canComboAttack = false;
            }
        }
    }
    void OnAnimatorIK()
    {
        if(playerState.state == PlayerState.State.Idle)
        {
            animator.SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, animator.GetFloat("HandWeight"));
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos.position);
        }
        else
        {
            animator.SetFloat("HandWeight", 0);
        }
    }
}
