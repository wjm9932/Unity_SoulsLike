using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Groggy : BossAttackAction, ICompositionNodeResettable
{
    private float groggyTime;
    private Action onResetCompositionNode;

    public Groggy(Blackboard blackBoard) : base(blackBoard)
    {
        groggyTime = 1f;
    }

    public override void OnEnter()
    {
        onResetCompositionNode();

        base.OnEnter();

        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().canAttack = false;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().ResetPath();
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetTrigger("isGroggy");

        SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.BOSS_GROGGY, 0.8f);
    }

    public override NodeState Execute()
    {
        if (blackboard.GetData<bool>("isGroggy") == true)
        {
            return NodeState.Running;
        }
        else
        {
            return NodeState.Success;
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        blackboard.GetData<GameObject>("Owner").GetComponent<BT_BossEnemy>().groggyAmount = 0f;
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetTrigger("isGroggyFinished");

    }
    public override void OnAnimatorIK()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 0f);
    }

    private IEnumerator TriggerEndGroggy()
    {
        yield return new WaitForSeconds(groggyTime);
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetTrigger("GroggyEnd");
    }
    public override void OnAnimationExit()
    {
        blackboard.SetData<bool>("isGroggy", false);
    }
    public override void OnAnimationEnter()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<BT_BossEnemy>().StartCoroutine(TriggerEndGroggy());
    }
    public void SetResetAction(Action resetAction)
    {
        this.onResetCompositionNode = resetAction;
    }
}