using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    void OnEnter();
    void OnExit();
    NodeState Execute();
}

public class ActionManager
{
    private IAction currentAction;

    public void ChangeAction(IAction newAction)
    {
        if (currentAction != newAction)
        {
            currentAction?.OnExit();
            currentAction = newAction;
            currentAction.OnEnter();
        }
    }

    public NodeState ExecuteCurrentAction()
    {
        return currentAction?.Execute() ?? NodeState.Failure;
    }
}
