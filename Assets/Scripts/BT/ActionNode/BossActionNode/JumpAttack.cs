using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpAttack : IAction, IAnimationEventHandler
{
    private Blackboard blackboard;
    private Quaternion dir;
    private bool isFirstFrame;
    public JumpAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        isFirstFrame = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance = 2f;

        dir = GetLookAtAngle();
        SetDashDestinationAndSpeed();

        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isJumpAttack", true);

        RegisterEvents();
    }

    public NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, dir, Time.deltaTime * 10);

        if(!isFirstFrame)
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
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isJumpAttack", false);
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
        var camera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        camera.GetComponent<CameraShake>().ShakeCamera();
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.JUMP_ATTACK, 0.6f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
        EffectManager.Instance.PlayEffect(blackboard.GetData<GameObject>("Owner").transform.position + blackboard.GetData<GameObject>("Owner").transform.forward * 2f, Vector3.up, blackboard.GetData<GameObject>("Owner").transform, ObjectPoolManager.ObjectType.DUST);
    }
    public void OnAnimationTransition()
    {
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_JUMP, 0.5f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
    }

    public void OnAnimationExit()
    {

    }
    public void OnAnimatorIK()
    {
        //blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
        //blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetIKPositionWeight(AvatarIKGoal.LeftHand, blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().GetFloat("HandWeight"));
        //blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetIKPosition(AvatarIKGoal.LeftHand, blackboard.GetData<GameObject>("Owner").GetComponent<BT_BossEnemy>().leftHandPos.position);

        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 0f);
    }

    private void SetDashDestinationAndSpeed()
    {
        float distance = Vector3.Distance(blackboard.GetData<GameObject>("Owner").transform.position, blackboard.GetData<GameObject>("target").transform.position);
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().SetDestination(blackboard.GetData<GameObject>("target").transform.position);

        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed = Mathf.Max(0, (distance - blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance) / 1f);
    }
    private Quaternion GetLookAtAngle()
    {
        Vector3 dir = blackboard.GetData<GameObject>("target").transform.position - blackboard.GetData<GameObject>("Owner").transform.position;
        dir.y = 0;

        return Quaternion.LookRotation(dir);
    }
}
