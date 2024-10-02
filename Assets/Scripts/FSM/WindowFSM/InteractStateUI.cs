using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractStateUI : IStateUI
{
    protected UIStateMachine sm;
    private float xCameraSpeed;
    private float yCameraSpeed;
    public InteractStateUI(UIStateMachine sm)
    {
        this.sm = sm;
        xCameraSpeed = sm.owner.lockOffCamera.m_XAxis.m_MaxSpeed;
        yCameraSpeed = sm.owner.lockOffCamera.m_YAxis.m_MaxSpeed;
    }

    // Start is called before the first frame update
    public virtual void Enter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        sm.owner.lockOffCamera.m_XAxis.m_MaxSpeed = 0f;
        sm.owner.lockOffCamera.m_YAxis.m_MaxSpeed = 0f;
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
        sm.owner.lockOffCamera.m_XAxis.m_MaxSpeed = xCameraSpeed;
        sm.owner.lockOffCamera.m_YAxis.m_MaxSpeed = yCameraSpeed;
    }
}
