using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractState : InteractState
{
    public QuestInteractState(PlayerMovementStateMachine sm) : base(sm)
    {

    }
    public override void Enter()
    {

    }
    public override void Update()
    {
        sm.owner.rb.velocity = Vector3.zero;

        if(Input.GetKeyDown(KeyCode.Escape) == true)
        {
            sm.ChangeState(sm.idleState);
        }
    }
    public override void Exit()
    {
        
    }
}
