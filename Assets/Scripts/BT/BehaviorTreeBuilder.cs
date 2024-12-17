using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BehaviorTreeBuilder : MonoBehaviour
{
    public Blackboard blackboard { get; private set; }
    public ActionManager actionManager { get; private set; }

    private CompositeNode currentNode;
    private Stack<CompositeNode> nodeStack = new Stack<CompositeNode>();

    private void Awake()
    {
        blackboard = new Blackboard();
        actionManager = new ActionManager();
    }

    public BehaviorTreeBuilder AddSelector()
    {
        var selector = new Selector();
        if (currentNode != null) nodeStack.Push(currentNode);
        currentNode = selector;
        return this;
    }

    public BehaviorTreeBuilder AddAttackSelector()
    {
        var selector = new AttackSelector();
        if (currentNode != null) nodeStack.Push(currentNode);
        currentNode = selector;
        return this;
    }

    public BehaviorTreeBuilder AddSequence()
    {
        var sequence = new Sequence();
        if (currentNode != null) nodeStack.Push(currentNode);
        currentNode = sequence;
        return this;
    }

    public BehaviorTreeBuilder AddCondition(Func<bool> condition)
    {
        currentNode.AddChild(new ConditionNode(condition));
        return this;
    }

    public BehaviorTreeBuilder AddAction(IAction action, ActionManager actionManager)
    {
        currentNode.AddChild(new ActionNode(action, actionManager));
        return this;
    }

    public BehaviorTreeBuilder EndComposite()
    {
        var finishedNode = currentNode;
        if (nodeStack.Count > 0)
        {
            currentNode = nodeStack.Pop();
            currentNode.AddChild(finishedNode);
        }
        return this;
    }

    public CompositeNode Build()
    {
        return currentNode;
    }
}
