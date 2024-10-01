using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateMachine
{
    public OpenInventoryState openInventoryState { get; }
    public CloseState closeState { get; }

    public IStateUI currentState { get; private set; }
    public Character character { get; }

    public UIStateMachine(Character character)
    {
        this.character = character;

        openInventoryState = new OpenInventoryState(this);
        closeState = new CloseState(this);
    }

    public void ChangeState(IStateUI newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }
    public void Update()
    {
        currentState?.Update();
    }
}
