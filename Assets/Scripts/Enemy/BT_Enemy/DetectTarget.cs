using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DetectTarget : MonoBehaviour
{
    private Blackboard blackboard;
    private Enemy owner;

    private void Start()
    {
        owner = GetComponent<Enemy>();
        blackboard = GetComponent<BehaviorTreeBuilder>().blackboard;

        StartCoroutine(DetectedPlayer());
    }

    private IEnumerator DetectedPlayer()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (blackboard.GetData<GameObject>("target") == null)
            {
                var target = blackboard.GetData<GameObject>("target") ?? IsTargetOnSight();
                if (target != null && target.GetComponent<LivingEntity>().isDead == false)
                {
                    blackboard.SetData<GameObject>("target", target);
                }
            }
            else
            {
                if(blackboard.GetData<GameObject>("target").GetComponent<LivingEntity>().isDead == true || IsPlayerOnNavMesh(blackboard.GetData<GameObject>("target")) == false)
                {
                    blackboard.SetData<GameObject>("target", null);
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private GameObject IsTargetOnSight()
    {
        var colliders = Physics.OverlapSphere(owner.eyeTransform.position, owner.viewDistance, owner.whatIsTarget);

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

            var direction = collider.transform.position - owner.eyeTransform.position;
            direction.y = owner.eyeTransform.forward.y;

            if (Vector3.Angle(direction, owner.eyeTransform.forward) > owner.fieldOfView * 0.5f)
            {
                return null;
            }

            RaycastHit hit;

            if (Physics.Raycast(owner.eyeTransform.position, direction, out hit, owner.viewDistance, owner.whatIsTarget) == true)
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
        return NavMesh.SamplePosition(target.transform.position, out hit, 0.1f, owner.navMesh.areaMask);
    }
}
