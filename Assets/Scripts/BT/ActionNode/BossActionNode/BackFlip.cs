using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BackFlip : BossAttackAction
{
    private Quaternion dir;
    public BackFlip(Blackboard blackBoard) : base(blackBoard)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = false;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance = 0f;

        dir = GetLookAtAngle();
        SetDashDestinationAndSpeed();
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isBackFlip", true);

    }

    public override NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, dir, Time.deltaTime * 30);
        return base.Execute(); 
    }

    public override void OnExit()
    {
        base.OnExit();
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isBackFlip", false);
    }
   

    public override void OnAnimationEnter()
    {
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_JUMP, 0.6f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
    }
    public override void OnAnimationTransition()
    {
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_FLIP, 0.3f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
        EffectManager.Instance.PlayEffect(blackboard.GetData<GameObject>("Owner").transform.position, Vector3.up, blackboard.GetData<GameObject>("Owner").transform, ObjectPoolManager.ObjectType.DUST);
    }
    public override void OnAnimationExit()
    {

    }
    public override void OnAnimatorIK()
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

}
