using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseState : IStateUI
{
    UIStateMachine sm;
    public CloseState(UIStateMachine sm)
    {
        this.sm = sm;
    }

    public void Enter()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q) == true) // Quest
        //{
        //    sm.ChangeState(sm.openState);
        //}

        if (Input.GetKeyDown(KeyCode.I) == true) // Inventory
        {
            sm.ChangeState(sm.openInventoryState);
        }
    }

    public void Exit()
    {

    }
}
