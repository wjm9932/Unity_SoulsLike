using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAction : IAction
{
    private Blackboard blackboard;
    GameObject player;
    private float testTime;
    public TestAction(Blackboard blackBoard, GameObject player)
    {
        this.blackboard = blackBoard;
        this.player = player;
    }

    public void OnEnter()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = true;
        blackboard.SetData<GameObject>("target", player);
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

