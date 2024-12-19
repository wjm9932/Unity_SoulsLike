using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BackFlip : IAction, IAnimationEventHandler
{
    private Blackboard blackboard;
    private Quaternion dir;
    private bool isFirstFrame;
    public BackFlip(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        isFirstFrame = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance = 0f;

        dir = GetLookAtAngle();
        SetDashDestinationAndSpeed();
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isBackFlip", true);


        RegisterEvents();
    }

    public NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, dir, Time.deltaTime * 30);

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
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isBackFlip", false);
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

    public void OnAnimationEnter()
    {
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_JUMP, 0.4f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
    }
    public void OnAnimationTransition()
    {
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_FLIP, 0.2f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
        EffectManager.Instance.PlayEffect(blackboard.GetData<GameObject>("Owner").transform.position, Vector3.up, blackboard.GetData<GameObject>("Owner").transform, ObjectPoolManager.ObjectType.DUST);
    }
    public void OnAnimationExit()
    {

    }
    public void OnAnimatorIK()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 0f);
    }

    private void SetDashDestinationAndSpeed()
    {
        Vector3 forwardDirection = dir * Vector3.forward;
        Vector3 backOffset = forwardDirection * -5;
        Vector3 dashDestination = blackboard.GetData<GameObject>("Owner").transform.position + backOffset;

        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().SetDestination(dashDestination);

        float distance = Vector3.Distance(blackboard.GetData<GameObject>("Owner").transform.position, dashDestination);
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed = distance / 0.75f;
    }
    private Quaternion GetLookAtAngle()
    {
        Vector3 dir = blackboard.GetData<GameObject>("target").transform.position - blackboard.GetData<GameObject>("Owner").transform.position;
        dir.y = 0;

        return Quaternion.LookRotation(dir);
    }
}
