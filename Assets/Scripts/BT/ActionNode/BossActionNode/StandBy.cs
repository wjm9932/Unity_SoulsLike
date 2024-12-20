using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StandBy : BossAttackAction
{
    private Quaternion dir;
    private bool isReadyToAttack;
    private IEnumerator timerCouroutine;
    public StandBy(Blackboard blackBoard) : base(blackBoard)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        dir = GetLookAtAngle();
        isReadyToAttack = false;
        timerCouroutine = GetReady();

        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isStabReady", true);
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().StartCoroutine(timerCouroutine);
    }

    public override NodeState Execute()
    {
        if (isReadyToAttack == false && Vector3.Distance(blackboard.GetData<GameObject>("Owner").transform.position, blackboard.GetData<GameObject>("target").transform.position) >= 3f)
        {
            blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, GetLookAtAngle(), Time.deltaTime * 10);
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
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isStabReady", false);

        if(timerCouroutine != null)
        {
            blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().StopCoroutine(timerCouroutine);
        }
    }
    public override void OnAnimatorIK()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 0f);
    }

    private IEnumerator GetReady()
    {
        yield return new WaitForSeconds(1f);
        timerCouroutine = null;
        isReadyToAttack = true;
    }
}
