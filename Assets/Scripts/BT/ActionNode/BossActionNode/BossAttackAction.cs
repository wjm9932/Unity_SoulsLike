using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class BossAttackAction : IAction, IAnimationEventHandler
{
    protected Blackboard blackboard;
    protected float animationEndPercent;
    private bool isFirstFrame;
    public BossAttackAction(Blackboard blackBoard, float animationEndPercent = 0.85f)
    {
        this.blackboard = blackBoard;
        this.animationEndPercent = animationEndPercent;
    }

    public virtual void OnEnter()
    {
        isFirstFrame = true;

        RegisterEvents();
    }
    public virtual NodeState Execute()
    {
        if (!isFirstFrame)
        {
            if (blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= animationEndPercent && blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().IsInTransition(0) == false)
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

    public virtual void OnExit()
    {
        RemoveEvents();
    }

    public void RegisterEvents()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationEnter += OnAnimationEnter;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationTransition += OnAnimationTransition;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationExit += OnAnimationExit;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().animationIK += OnAnimatorIK;
    }

    public void RemoveEvents()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationEnter -= OnAnimationEnter;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationTransition -= OnAnimationTransition;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationExit -= OnAnimationExit;
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

    protected Quaternion GetLookAtAngle()
    {
        Vector3 dir = blackboard.GetData<GameObject>("target").transform.position - blackboard.GetData<GameObject>("Owner").transform.position;
        dir.y = 0;

        return Quaternion.LookRotation(dir);
    }
}
