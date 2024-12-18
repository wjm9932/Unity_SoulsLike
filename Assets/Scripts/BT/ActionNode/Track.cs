using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Track : IAction
{
    private Blackboard blackboard;
    private IEnumerator trackCouroutine;
    private NodeState state;
    public Track(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        state = NodeState.Running;
    }

    public void OnEnter()
    {
        trackCouroutine = TrackTarget();

        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = false;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().avoidancePriority = 51;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed = blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().trackingSpeed;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance = blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().trackingStopDistance;

        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().animator.SetFloat("Speed", blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed);
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().StartCoroutine(trackCouroutine);

    }

    public NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, GetMoveRotationAngle(), Time.deltaTime * 5);
        return state;
    }

    public void OnExit()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().StopCoroutine(trackCouroutine);
    }

    IEnumerator TrackTarget()
    {
        while (blackboard.GetData<GameObject>("Owner").GetComponent<LivingEntity>().isDead == false && blackboard.GetData<GameObject>("target") != null)
        {
            if (Vector3.Distance(blackboard.GetData<GameObject>("target").transform.position, blackboard.GetData<GameObject>("Owner").transform.position) >= blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().viewDistance)
            {
                blackboard.SetData<GameObject>("target", null);
                state = NodeState.Success;
            }

            yield return new WaitForSeconds(0.05f);

            blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().SetDestination(blackboard.GetData<GameObject>("target").transform.position);
            state = NodeState.Running;
        }
    }

    private Quaternion GetMoveRotationAngle()
    {
        Vector3 direction = blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().velocity;
        direction.y = 0;

        if (direction == Vector3.zero)
        {
            return Quaternion.identity;
        }

        return Quaternion.LookRotation(direction);
    }
}