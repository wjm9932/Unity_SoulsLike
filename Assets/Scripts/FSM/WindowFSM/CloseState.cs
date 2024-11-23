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
        sm.owner.input.canGetMouseInput = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
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

        if(sm.owner.input.isPressingEscape == true)
        {
            sm.ChangeState(sm.openPauseMenuState);
        }
    }

    public void Exit()
    {

    }
}
