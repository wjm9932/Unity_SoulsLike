using System.Collections.Generic;
public abstract class CompositeNode : INode
{
    protected List<INode> children = new List<INode>();

    public void AddChild(INode child)
    {
        children.Add(child);
    }

    public abstract NodeState Evaluate();
}

