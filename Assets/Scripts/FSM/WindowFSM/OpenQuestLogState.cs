using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
public class OpenQuestLogState : OpenState
{
    public OpenQuestLogState(UIStateMachine sm) : base(sm)
    {

    }

    public override void Enter()
    {
        base.Enter();
        sm.owner.playerQuestManager.questLogUI.SetActive(true);
    }
    public override void Update()
    {
        base.Update();
        if(sm.owner.input.isPressingQuestLogKey == true)
        {
            sm.ChangeState(sm.closeState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        sm.owner.playerQuestManager.questLogUI.SetActive(false);
    }
}
