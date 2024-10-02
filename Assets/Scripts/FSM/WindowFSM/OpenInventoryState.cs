using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
public class OpenInventoryState : OpenState
{
    public OpenInventoryState(UIStateMachine sm) : base(sm)
    {

    }

    public override void Enter()
    {
        base.Enter();
        sm.owner.inventoryUI.SetActive(true);
    }
    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        base.Exit();
        sm.owner.inventoryUI.SetActive(false);
    }
}
