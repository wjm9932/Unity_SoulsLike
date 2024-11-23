using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPauseMenuState : OpenState
{
    public OpenPauseMenuState(UIStateMachine sm) : base(sm)
    {

    }

    public override void Enter()
    {
        base.Enter();
        sm.owner.pauseMenu.SetActive(true);
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        base.Exit();
        sm.owner.pauseMenu.SetActive(false);
    }
}
