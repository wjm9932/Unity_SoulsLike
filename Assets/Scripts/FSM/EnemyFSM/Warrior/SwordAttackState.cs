using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM
{
    public class SwordAttackState : EnemyPatternState
    {
        public  SwordAttackState(EnemyBehaviorStateMachine sm) : base(sm)
        {

        }

        public override void Enter()
        {
            sm.owner.navMesh.isStopped = true;
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
    }
}


