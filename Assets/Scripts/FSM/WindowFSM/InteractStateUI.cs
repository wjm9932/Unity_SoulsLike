using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractStateUI : OpenState
{
    public InteractStateUI(UIStateMachine sm) : base(sm)
    {
        this.sm = sm;
    }

    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
