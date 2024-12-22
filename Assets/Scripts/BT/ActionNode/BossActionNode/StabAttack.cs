using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StabAttack : BossAttackAction
{
    private Quaternion dir;
    public StabAttack(Blackboard blackBoard) : base(blackBoard, 0.75f)
    {
        
    }

    public override void OnEnter()
    {
        base.OnEnter();

        dir = GetLookAtAngle();

        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().SetDamage(10f);
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isStab", true);
    }

    public override NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, dir, Time.deltaTime * 10);

        return base.Execute();
    }

    public override void OnExit()
    {
        base.OnExit();

        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isStab", false);
    }

    public override void OnAnimationEnter()
    {

    }
    public override void OnAnimationTransition()
    {
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_STAB_ATTACK, 0.3f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
    }

    public override void OnAnimationExit()
    {

    }
    public override void OnAnimatorIK()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 0f);
    }
}