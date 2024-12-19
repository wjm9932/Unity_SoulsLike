using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSequence : CompositeNode
{
    private int currentIndex;
    private INode currentNode;
    public override NodeState Evaluate()
    {
        if (currentNode == null)
        {
            if (currentIndex < children.Count)
            {
                currentNode = children[currentIndex];
            }
            else
            {
                currentIndex = 0;
                return NodeState.Success;
            }
        }

        NodeState state = currentNode.Evaluate();

        if (state == NodeState.Running)
        {
            return NodeState.Running;
        }

        if (state == NodeState.Success)
        {
            currentIndex++;
            currentNode = null;
            return NodeState.Running; 
        }

        ResetNodeInfo();
        return NodeState.Failure;
    }

    public override void Reset()
    {
        base.Reset();
        ResetNodeInfo();
    }

    private void ResetNodeInfo()
    {
        currentNode = null;
        currentIndex = 0;
    }
}