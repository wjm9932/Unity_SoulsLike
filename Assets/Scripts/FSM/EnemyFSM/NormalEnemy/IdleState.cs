using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            sm.owner.StartCoroutine(updatePathCoroutine);
        }
        public override void Update()
        {
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
        private IEnumerator UpdatePath()
        {
            while(sm.owner.isDead == false)
            {
                if (IsTargetOnSight() == true)
                {
                    //sm.ChangeState(sm.patrolState);
                }
                else
                {

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


