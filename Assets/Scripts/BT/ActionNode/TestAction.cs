using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAction : IAction
{
    private Blackboard blackboard;
    private float testTime;
    public TestAction(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = true;
        testTime = 0f;
    }

    public NodeState Execute()
    {
        testTime += Time.deltaTime;
        if (testTime >= 2f)
        {
            Debug.Log("Success");
            return NodeState.Success;
        }
        else
        {
            Debug.Log("Running");
            return NodeState.Running;
        }
    }

    public void OnExit()
    {

    }

}

