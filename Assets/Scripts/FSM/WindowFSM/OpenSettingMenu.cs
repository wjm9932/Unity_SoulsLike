using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSettingMenu : OpenState
{
    public OpenSettingMenu(UIStateMachine sm) : base(sm)
    {

    }

    public override void Enter()
    {
        base.Enter();
        MenuManager.Instance.settingMenu.SetActive(true);
    }
    public override void Update()
    {
        if(sm.owner.input.isPressingEscape == true)
        {
            sm.ChangeState(sm.openPauseMenuState);
        }
    }
    public override void Exit()
    {
        MenuManager.Instance.settingMenu.SetActive(false);
    }
}

