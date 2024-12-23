using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DashAttack : BossAttackAction
{
    private float timer;
    public DashAttack(Blackboard blackBoard) : base(blackBoard)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();

        timer = 0.1f;

        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().SetDamage(30f);
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = false;
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isDashStab", true);
        blackboard.GetData<GameObject>("Owner").GetComponent<LivingEntity>().SetCanAttack(1);
        blackboard.GetData<GameObject>("Owner").GetComponent<CapsuleCollider>().isTrigger = true;
        blackboard.SetData<bool>("isAttacking", true);

        SetDashDestination();

        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_DASH, 0.6f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
    }

    public override NodeState Execute()
    {
        if (blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().remainingDistance <= 0f)
        {
            timer -= Time.deltaTime;
        }

        if(timer <= 0f)
        {
            blackboard.SetData<bool>("isAttacking", false);
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public override void OnExit()
    {
        base.OnExit();
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isDashStab", false);
        blackboard.GetData<GameObject>("Owner").GetComponent<CapsuleCollider>().isTrigger = false;
        blackboard.GetData<GameObject>("Owner").GetComponent<LivingEntity>().SetCanAttack(0);
    }
    public override void OnAnimatorIK()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 0f);
    }

    private void SetDashDestination()
    {
        float distance = Vector3.Distance(blackboard.GetData<GameObject>("Owner").transform.position, blackboard.GetData<GameObject>("target").transform.position);

        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance = 0f;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed = distance / 0.15f;

        Vector3 backOffset = blackboard.GetData<GameObject>("Owner").transform.forward * 5f;
        Vector3 dashDestination = blackboard.GetData<GameObject>("target").transform.position + backOffset;

        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().SetDestination(dashDestination);
    }
}
