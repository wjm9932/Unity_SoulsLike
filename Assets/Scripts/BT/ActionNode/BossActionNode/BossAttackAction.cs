using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class BossAttackAction : IAction, IAnimationEventHandler
{
    private Blackboard blackboard;
    private bool isFirstFrame;
    public BossAttackAction(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        isFirstFrame = true;

        RegisterEvents();
    }
    public NodeState Execute()
    {
        if (!isFirstFrame)
        {
            if (blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f && blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().IsInTransition(0) == false)
            {
                return NodeState.Success;
            }
            else
            {
                return NodeState.Running;
            }
        }
        else
        {
            isFirstFrame = false;
            return NodeState.Running;
        }
    }

    public void OnExit()
    {
        RemoveEvents();
    }

    public void RegisterEvents()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationEnter += OnAnimationEnter;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationTransition += OnAnimationTransition;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().animationIK += OnAnimatorIK;
    }

    public void RemoveEvents()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationEnter -= OnAnimationEnter;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationTransition -= OnAnimationTransition;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().animationIK -= OnAnimatorIK;
    }

    public virtual void OnAnimationEnter()
    {
        
    }
    public virtual void OnAnimationTransition()
    {

    }
    public virtual void OnAnimationExit()
    {

    }
    public virtual void OnAnimatorIK()
    {
      
    }
}
