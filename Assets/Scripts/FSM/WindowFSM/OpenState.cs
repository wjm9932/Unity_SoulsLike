using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OpenState : IStateUI
{
    protected UIStateMachine sm;
    private float xCameraSpeed;
    private float yCameraSpeed;
    public OpenState(UIStateMachine sm)
    {
        this.sm = sm;
        xCameraSpeed = sm.character.lockOffCamera.m_XAxis.m_MaxSpeed;
        yCameraSpeed = sm.character.lockOffCamera.m_YAxis.m_MaxSpeed;
    }

    // Start is called before the first frame update
    public virtual void Enter()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        sm.character.lockOffCamera.m_XAxis.m_MaxSpeed = 0f;
        sm.character.lockOffCamera.m_YAxis.m_MaxSpeed = 0f;
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
        sm.character.lockOffCamera.m_XAxis.m_MaxSpeed = xCameraSpeed;
        sm.character.lockOffCamera.m_YAxis.m_MaxSpeed = yCameraSpeed;
    }
}
