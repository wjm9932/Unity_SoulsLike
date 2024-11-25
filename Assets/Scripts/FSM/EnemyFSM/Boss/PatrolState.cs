using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BossEnemyFSM
{
    public class PatrolState : BossEnemyPatternState
    {
        private IEnumerator updatePathCoroutine;
        public PatrolState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 2f;
            agentSpeed = 2f;
        }

        public override void Enter()
        {
            sm.owner.navMesh.isStopped = false;
            sm.owner.navMesh.speed = agentSpeed;
            sm.owner.navMesh.stoppingDistance = stoppingDistance;

            updatePathCoroutine = UpdatePath();
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
        private Quaternion GetMoveRotationAngle()
        {
            Vector3 direction = sm.owner.navMesh.velocity;
            direction.y = 0;

            return Quaternion.LookRotation(direction);
        }
        private IEnumerator UpdatePath()
        {
            while (sm.owner.isDead == false && sm.currentState == this)
            {
                if(sm.owner.target != null)
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
    }
}


