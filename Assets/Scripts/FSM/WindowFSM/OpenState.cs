using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OpenState : IStateUI
{
    protected UIStateMachine sm;
    public OpenState(UIStateMachine sm)
    {
        this.sm = sm;
    }

    // Start is called before the first frame update
    public virtual void Enter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        sm.owner.input.canGetMouseInput = false;
        SoundManager.Instance.Play2DSoundEffect(SoundManager.UISoundEffectType.CLICK, 0.5f);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            sm.ChangeState(sm.closeState);
        }
    }

    public virtual void Exit()
    {
    }
}
