using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyFSM
{
    public class PatrolState : EnemyPatternState
    {
        private IEnumerator updatePathCoroutine;
        public PatrolState(EnemyBehaviorStateMachine sm) : base(sm)
        {

        }

        public override void Enter()
        {
            sm.owner.target = null;
            sm.owner.navMesh.isStopped = false;
            sm.owner.navMesh.speed = sm.owner.patrolSpeed;
            sm.owner.navMesh.stoppingDistance = 1f;
            sm.owner.animator.SetFloat("Speed", sm.owner.navMesh.speed);
            sm.owner.navMesh.avoidancePriority = 51;

            updatePathCoroutine = UpdatePath();
            sm.owner.StartCoroutine(updatePathCoroutine);
        }
        public override void Update()
        {
            base.Update();
        }
        public override void PhysicsUpdate()
        {

        }
        public override void LateUpdate()
        {

        }
        public override void Exit()
        {
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
        private Vector3 GetDestination()
        {
            NavMeshHit hit;
            Vector3 randomDirection = Random.insideUnitSphere * 10f + sm.owner.transform.position;

            if (NavMesh.SamplePosition(randomDirection, out hit, 10f, sm.owner.navMesh.areaMask) == true)
            {
                return hit.position;
            }
            else
            {
                return sm.owner.transform.transform.position;
            }
        }

        private IEnumerator UpdatePath()
        {
            while (sm.owner.isDead == false && sm.currentState == this)
            {
                if (IsTargetOnSight() == true)
                {
                    sm.ChangeState(sm.trackingState);
                }
                else
                {
                    if (sm.owner.navMesh.remainingDistance <= sm.owner.navMesh.stoppingDistance)
                    {
                        sm.owner.navMesh.SetDestination(GetDestination());
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
                if(collider.gameObject.GetComponent<LivingEntity>().isDead == true)
                {
                    return false;
                }

                if (IsPlayerOnNavMesh(collider.gameObject) == false)
                {
                    return false;
                }

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

        bool IsPlayerOnNavMesh(GameObject target)
        {
            NavMeshHit hit;
            return NavMesh.SamplePosition(target.transform.position, out hit, 0.1f, sm.owner.navMesh.areaMask);
        }
    }
}


