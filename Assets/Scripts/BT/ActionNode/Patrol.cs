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
        blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = false;
        blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().animator.SetFloat("Speed", blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed);

        blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().StartCoroutine(updatePathCoroutine);
        //Debug.Log("Entering Patrol state"); 
    }
    
    public NodeState Execute()
    {
        //Debug.Log("Executing Patrol");
        blackBoard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackBoard.GetData<GameObject>("Owner").transform.rotation, GetMoveRotationAngle(), Time.deltaTime * 5);
        return NodeState.Success;
    }

    public void OnExit() 
    {
        blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().StopCoroutine(updatePathCoroutine);
        //Debug.Log("Exiting Patrol state"); 
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
            if (IsTargetOnSight() == true)
            {
                blackBoard.SetData<bool>("isTargetOnSight", true);
                //sm.ChangeState(sm.trackingState);
            }
            else
            {
                if (blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().remainingDistance <= blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance)
                {
                    blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().SetDestination(GetDestination());
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private bool IsTargetOnSight()
    {
        Transform eyeTransform = blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().eyeTransform;
        float viewDistance = blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().viewDistance;
        var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().whatIsTarget);

        foreach (var collider in colliders)
        {
            if (collider.gameObject.GetComponent<LivingEntity>().isDead == true)
            {
                return false;
            }

            if (IsPlayerOnNavMesh(collider.gameObject) == false)
            {
                return false;
            }

            var direction = collider.transform.position - eyeTransform.position;
            direction.y = eyeTransform.forward.y;

            if (Vector3.Angle(direction, eyeTransform.forward) > blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().fieldOfView * 0.5f)
            {
                return false;
            }

            RaycastHit hit;

            if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().whatIsTarget) == true)
            {
                if (hit.transform == collider.transform)
                {

                    blackBoard.SetData<GameObject>("target", collider.gameObject);
                    //sm.owner.target = collider.gameObject;
                    return true;
                }
            }
        }

        return false;
    }

    bool IsPlayerOnNavMesh(GameObject target)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(target.transform.position, out hit, 0.1f, blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().areaMask);
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
