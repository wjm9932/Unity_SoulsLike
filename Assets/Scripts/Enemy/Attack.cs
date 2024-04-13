using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private bool canAttack;
    // Start is called before the first frame update
    void Start()
    {
        canAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Character character = other.GetComponent<Character>();

            if (canAttack == true && character.canBeDamaged == true)
            {
                character.animator.SetTrigger("Hit");
                character.playerMovementStateMachine.ChangeState(character.playerMovementStateMachine.hitState);
            }
        }
    }

    public void SetCanAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }
}
