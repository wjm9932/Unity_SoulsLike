using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : CompositeNode
{
    int randomIndex;
    INode runningNode;
    public override NodeState Evaluate()
    {
        if (runningNode != null)
        {
            NodeState runningState = runningNode.Evaluate();

            if (runningState == NodeState.Running)
            {
                return NodeState.Running;
            }
            runningNode = null;
            if (runningState == NodeState.Success)
            {
                return NodeState.Success;
            }
        }

        foreach (var child in children)
        {
            NodeState state = child.Evaluate();

            if (state == NodeState.Running)
            {
                runningNode = child;
                return NodeState.Running;
            }

            if (state == NodeState.Success)
            {
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }
}
