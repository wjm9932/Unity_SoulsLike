using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private PlayerInput input;
    private PlayerState playerState;
    private Animator animator;
    private ComboAttack currentCombo;
    private bool canComboAttack = true;
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
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
        playerState = GetComponent<PlayerState>();
    }

    // Update is called once per frame
    void Update()
    {
        if(input.isAttack == true)
        {
            SwordAttack();
        }
        ResetCombo();
    }
    void ResetCombo()
    {
        if (playerState.state == PlayerState.State.Idle && currentCombo != ComboAttack.Idle)
        {
            if(currentCombo != ComboAttack.Attack2)
            {
                animator.SetTrigger("ResetAttackCombo");
            }
            currentCombo = ComboAttack.Idle;
            canComboAttack = true;
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
        }
    }
}
