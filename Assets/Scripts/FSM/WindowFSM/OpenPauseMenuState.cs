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
        MenuManager.Instance.pauseMenu.SetActive(true);
        MenuManager.Instance.menuList.SetActive(true);
    }
    public override void Update()
    {
        if (sm.owner.input.isPressingEscape == true)
        {
            sm.ChangeState(sm.closeState);
            MenuManager.Instance.pauseMenu.SetActive(false);
            SoundManager.Instance.Play2DSoundEffect(SoundManager.UISoundEffectType.CLICK, 0.3f);
        }
    }
    public override void Exit()
    {
        MenuManager.Instance.menuList.SetActive(false);
    }
}
