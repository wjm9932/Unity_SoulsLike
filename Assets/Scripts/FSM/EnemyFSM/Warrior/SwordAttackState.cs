using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM
{
    public class SwordAttackState : EnemyPatternState
    {
        Quaternion dir;
        private bool isDone;
        public  SwordAttackState(EnemyBehaviorStateMachine sm) : base(sm)
        {

        }

        public override void Enter()
        {
            isDone = false;
            dir = GetLookAtAngle();
            sm.owner.navMesh.isStopped = true;
            sm.owner.animator.SetBool("IsSwordAttack", true);
            sm.owner.SetDamage(10f);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 10);

            if(isDone == true)
            {
                sm.ChangeState(sm.trackingState);
            }

        }
        public override void PhysicsUpdate()
        {

        }
        public override void LateUpdate()
        {

        }
        public override void Exit()
        {
            sm.owner.animator.SetBool("IsSwordAttack", false);
        }
        public override void OnAnimationEnterEvent()
        {

        }
        public override void OnAnimationExitEvent()
        {
            isDone = true;
        }
        public override void OnAnimationTransitionEvent()
        {

        }
        public override void OnAnimatorIK()
        {

        }
    }
}


