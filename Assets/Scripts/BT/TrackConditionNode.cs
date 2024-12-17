using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class TrackConditionNode : INode
{
    private Blackboard blackBoard;

    public TrackConditionNode(Blackboard blackboard)
    {
        this.blackBoard = blackboard;
    }

    public NodeState Evaluate()
    {
        if(blackBoard.GetData<GameObject>("target") == null)
        {
            GameObject target = IsTargetOnSight();

            if (target != null)
            {
                blackBoard.SetData<GameObject>("target", target);
                return NodeState.Success;
            }
            else
            {
                return NodeState.Failure;
            }
        }
        else
        {
            return NodeState.Success;
        }
    }

    private GameObject IsTargetOnSight()
    {
        Transform eyeTransform = blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().eyeTransform;
        float viewDistance = blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().viewDistance;
        var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().whatIsTarget);

        foreach (var collider in colliders)
        {
            if (collider.gameObject.GetComponent<LivingEntity>().isDead == true)
            {
                return null;
            }

            if (IsPlayerOnNavMesh(collider.gameObject) == false)
            {
                return null;
            }

            var direction = collider.transform.position - eyeTransform.position;
            direction.y = eyeTransform.forward.y;

            if (Vector3.Angle(direction, eyeTransform.forward) > blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().fieldOfView * 0.5f)
            {
                return null;
            }

            RaycastHit hit;

            if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, blackBoard.GetData<GameObject>("Owner").GetComponent<Enemy>().whatIsTarget) == true)
            {
                if (hit.transform == collider.transform)
                {
                    return collider.transform.gameObject;
                }
            }
        }

        return null;
    }

    bool IsPlayerOnNavMesh(GameObject target)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(target.transform.position, out hit, 0.1f, blackBoard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().areaMask);
    }
}
