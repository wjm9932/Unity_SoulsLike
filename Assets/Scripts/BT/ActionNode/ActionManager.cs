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
    private bool isDone = false;
    public void ChangeAction(IAction newAction)
    {
        if (currentAction != newAction || isDone == true)
        {
            currentAction?.OnExit();
            currentAction = newAction;
            currentAction.OnEnter();
            isDone = false;
        }
    }

    public NodeState ExecuteCurrentAction()
    {
        if (currentAction == null)
            return NodeState.Failure;

        var state = currentAction.Execute();

        if (state == NodeState.Success)
        {
            isDone = true;
            return NodeState.Success;
        }

        return state;
    }
}
