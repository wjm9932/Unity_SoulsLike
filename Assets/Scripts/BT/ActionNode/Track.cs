using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Track : IAction
{
    private Blackboard blackBoard;
    private IEnumerator trackCouroutine;
    public Track(Blackboard blackBoard)
    {
        this.blackBoard = blackBoard;
    }

    public void OnEnter()
    {
        trackCouroutine = TrackTarget();

        blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = false;
        blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().avoidancePriority = 51;
        blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed = blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().trackingSpeed;
        blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance = blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().trackingStopDistance;

        blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().animator.SetFloat("Speed", blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed);
        blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().StartCoroutine(trackCouroutine);

    }

    public NodeState Execute()
    {
        blackBoard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackBoard.GetData<GameObject>("Owner").transform.rotation, GetMoveRotationAngle(), Time.deltaTime * 5);
        return NodeState.Success;
    }

    public void OnExit()
    {
        blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().StopCoroutine(trackCouroutine);
    }

    IEnumerator TrackTarget()
    {
        while (blackBoard.GetData<GameObject>("Owner").GetComponent<LivingEntity>().isDead == false && blackBoard.GetData<GameObject>("target") != null)
        {
            if (blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().remainingDistance >= blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().viewDistance)
            {
                blackBoard.SetData<GameObject>("target", null);
            }

            yield return new WaitForSeconds(0.05f);

            blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().SetDestination(blackBoard.GetData<GameObject>("target").transform.position);
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