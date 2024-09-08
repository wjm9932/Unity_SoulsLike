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
        sm.character.lockOffCamera.m_XAxis.m_InputAxisName = "";
        sm.character.lockOffCamera.m_YAxis.m_InputAxisName = "";
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
        sm.character.lockOffCamera.m_XAxis.m_InputAxisName = "Mouse X";
        sm.character.lockOffCamera.m_YAxis.m_InputAxisName = "Mouse Y";
    }
}
