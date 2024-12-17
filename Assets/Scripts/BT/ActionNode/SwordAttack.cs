using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordAttack : IAction
{
    private Blackboard blackboard;
    private Quaternion dir;
    public SwordAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().SetDamage(10f);

        dir = GetLookAtAngle();
    }

    public NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, dir, Time.deltaTime * 10);
        
        if (blackboard.GetData<bool>("isAttacking") == true)
        {
            return NodeState.Running;
        }
        else
        {
            return NodeState.Success;
        }
    }

    public void OnExit()
    {
        blackboard.SetData<bool>("isAttacking", false);
    }

    private Quaternion GetLookAtAngle()
    {
        Vector3 dir = blackboard.GetData<GameObject>("target").transform.position - blackboard.GetData<GameObject>("Owner").transform.position;
        dir.y = 0;

        return Quaternion.LookRotation(dir);
    }
}
