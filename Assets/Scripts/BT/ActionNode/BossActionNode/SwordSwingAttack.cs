using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordSwingAttack : BossAttackAction
{
    private Quaternion dir;
    public SwordSwingAttack(Blackboard blackBoard) : base(blackBoard)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        dir = GetLookAtAngle();

        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().SetDamage(20f);
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isAttack", true);
    }

    public override NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, dir, Time.deltaTime * 10);

        return base.Execute();
    }

    public override void OnExit()
    {
        base.OnExit();
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isAttack", false);
    }

    public override void OnAnimationTransition()
    {
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_SWORD_ATTACK, 0.35f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
    }

    public override void OnAnimatorIK()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetIKPositionWeight(AvatarIKGoal.LeftHand, blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().GetFloat("HandWeight"));
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetIKPosition(AvatarIKGoal.LeftHand, blackboard.GetData<GameObject>("Owner").GetComponent<BT_BossEnemy>().leftHandPos.position);
    }
}

