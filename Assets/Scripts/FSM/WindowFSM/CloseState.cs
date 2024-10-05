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

        if (sm.owner.input.isPressingQuestLogKey == true) // Quest
        {
            sm.ChangeState(sm.openQuestLogState);
        }

        if (sm.owner.input.isInteracting == true)
        {
            if (QuestManager.Instance.InteractWithQuest() == true)
            {
                sm.ChangeState(sm.questInteractState);
            }
        }

        if (sm.owner.input.isPressingInventoryKey == true) // Inventory
        {
            sm.ChangeState(sm.openInventoryState);
        }
    }

    public void Exit()
    {

    }
}
