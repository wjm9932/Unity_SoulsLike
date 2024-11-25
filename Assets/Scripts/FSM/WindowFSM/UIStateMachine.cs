using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateMachine
{
    public OpenInventoryState openInventoryState { get; }
    public OpenQuestLogState openQuestLogState { get; }
    public QuestInteractState questInteractState { get; }
    public OpenPauseMenuState openPauseMenuState { get; }
    public CloseState closeState { get; }

    public IStateUI currentState { get; private set; }
    public Character owner { get; }

    public UIStateMachine(Character character)
    {
        this.owner = character;

        openInventoryState = new OpenInventoryState(this);
        openQuestLogState = new OpenQuestLogState(this);
        questInteractState = new QuestInteractState(this);
        openPauseMenuState = new OpenPauseMenuState(this);
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
