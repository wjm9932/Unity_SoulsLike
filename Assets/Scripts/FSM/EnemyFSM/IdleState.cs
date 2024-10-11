using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NormalEnemyFSM
{
    public class IdleState : EnemyPatternState
    {
        private IEnumerator updatePathCoroutine;
        public IdleState(EnemyBehaviorStateMachine sm) : base(sm)
        {

        }

        public override void Enter()
        {
            updatePathCoroutine = UpdatePath();
            sm.owner.navMesh.isStopped = false;
            sm.owner.navMesh.speed = 1f;
            sm.owner.navMesh.stoppingDistance = 1f;
            sm.owner.StartCoroutine(updatePathCoroutine);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, GetMoveRotationAngle(), Time.deltaTime * 5);
        }
        public override void PhysicsUpdate()
        {

        }
        public override void LateUpdate()
        {

        }
        public override void Exit()
        {
            sm.owner.navMesh.isStopped = true;
            sm.owner.StopCoroutine(updatePathCoroutine);
            sm.owner.navMesh.ResetPath();
        }
        public override void OnAnimationEnterEvent()
        {

        }
        public override void OnAnimationExitEvent()
        {

        }
        public override void OnAnimationTransitionEvent()
        {

        }
        public override void OnAnimatorIK()
        {

        }
        private Vector3 GetSpawnPosition()
        {
            NavMeshHit hit;
            Vector3 randomDirection = Random.insideUnitSphere * 10f + sm.owner.transform.position;

            if (NavMesh.SamplePosition(randomDirection, out hit, 10f, sm.owner.navMesh.areaMask) == true)
            {
                return hit.position;
            }
            else
            {
                Debug.LogError("cannot find valid position");
                return sm.owner.transform.transform.position;
            }
        }

        private IEnumerator UpdatePath()
        {
            while(sm.owner.isDead == false)
            {
                if (IsTargetOnSight() == true)
                {
                    if(sm.owner.entityType == EntityType.ENEMY)
                    {
                        sm.ChangeState(sm.trackingState);
                    }
                    else if(sm.owner.entityType == EntityType.ARCHER)
                    {
                        sm.ChangeState(sm.shootArrowState);
                    }
                }
                else
                {
                    if (sm.owner.navMesh.remainingDistance <= sm.owner.navMesh.stoppingDistance)
                    {
                        sm.owner.navMesh.SetDestination(GetSpawnPosition());
                    }
                }
                yield return new WaitForSeconds(0.05f);
            }
        }

        private bool IsTargetOnSight()
        {
            Transform eyeTransform = sm.owner.eyeTransform;
            float viewDistance = sm.owner.viewDistance;
            var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, sm.owner.whatIsTarget);

            foreach (var collider in colliders)
            {
                var direction = collider.transform.position - eyeTransform.position;
                direction.y = eyeTransform.forward.y;

                if (Vector3.Angle(direction, eyeTransform.forward) > sm.owner.fieldOfView * 0.5f) 
                {
                    return false;
                }

                RaycastHit hit;

                if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, sm.owner.whatIsTarget) == true)
                {
                    if (hit.transform == collider.transform)
                    {
                        sm.owner.target = collider.gameObject;
                        return true;
                    }
                }
            }

            return false;
        }


    }
}


