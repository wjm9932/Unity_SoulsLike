using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM
{
    public class HitState : EnemyPatternState
    {
        private bool isDone;
        public HitState(EnemyBehaviorStateMachine sm) : base(sm)
        {

        }

        public override void Enter()
        {
            isDone = false;
            sm.owner.animator.SetBool("IsHit", true);
            sm.owner.canBeDamaged = false;
            sm.owner.navMesh.isStopped = true;
        }
        public override void Update()
        {
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
            sm.owner.canBeDamaged = true;
            sm.owner.animator.SetBool("IsHit", false);
        }
        public override void OnAnimationEnterEvent()
        {

        }
        public override void OnAnimationExitEvent()
        {

        }
        public override void OnAnimationTransitionEvent()
        {
            isDone = true;
        }
        public override void OnAnimatorIK()
        {

        }
    }
}


