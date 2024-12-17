using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : IAction
{
    private Blackboard blackBoard;
    private IEnumerator updatePathCoroutine;
    public Patrol(Blackboard blackBoard)
    {
        this.blackBoard = blackBoard;
    }

    public void OnEnter() 
    {
        updatePathCoroutine = UpdatePath();
        blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed = 1f;
        blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance = 1f;
        blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().avoidancePriority = 51;
        blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = false;
        blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().animator.SetFloat("Speed", blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed);

        blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().StartCoroutine(updatePathCoroutine);
    }
    
    public NodeState Execute()
    {
        blackBoard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackBoard.GetData<GameObject>("Owner").transform.rotation, GetMoveRotationAngle(), Time.deltaTime * 5);
        return NodeState.Success;
    }

    public void OnExit() 
    {
        blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().StopCoroutine(updatePathCoroutine);
    }

    private Vector3 GetDestination()
    {
        NavMeshHit hit;
        
        Vector3 randomDirection = Random.insideUnitSphere * 10f + blackBoard.GetData<GameObject>("Owner").transform.position;

        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().areaMask) == true)
        {
            return hit.position;
        }
        else
        {
            return blackBoard.GetData<GameObject>("Owner").transform.transform.position;
        }
    }

    private IEnumerator UpdatePath()
    {
        while (blackBoard.GetData<GameObject>("Owner").GetComponent<LivingEntity>().isDead == false)
        {
            if (blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().remainingDistance <= blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance)
            {
                blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().SetDestination(GetDestination());
            }

            yield return new WaitForSeconds(0.05f);
        }
    }



    private Quaternion GetMoveRotationAngle()
    {
        Vector3 direction = blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().velocity;
        direction.y = 0;

        if (direction == Vector3.zero)
        {
            return Quaternion.identity;
        }

        return Quaternion.LookRotation(direction);
    }
}
