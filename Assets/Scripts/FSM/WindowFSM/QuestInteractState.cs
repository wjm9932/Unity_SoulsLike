using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractState : InteractStateUI
{
    public QuestInteractState(UIStateMachine sm) : base(sm)
    {

    }

    // Update is called once per frame
    public override void Enter()
    {
        base.Enter();
        sm.owner.playerQuestManager.questDialogueUI.SetActive(true);
    }
    public override void Update()
    {
        base.Update();
        if(sm.owner.input.isInteracting == true)
        {
            sm.ChangeState(sm.closeState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        sm.owner.playerQuestManager.questDialogueUI.SetActive(false);
    }
}
