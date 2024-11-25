using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossEnemyFSM
{
    public class StabAttackState : BossEnemyPatternState
    {
        public StabAttackState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            dir = GetLookAtAngle();

            sm.owner.canAttack = true;
            sm.owner.navMesh.isStopped = true;
            sm.owner.animator.SetTrigger("Stab");
            sm.owner.SetDamage(5f);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 10);
            if (sm.owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f && sm.owner.animator.IsInTransition(0) == false)
            {
                GetBossPattern();
            }
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
            sm.owner.canAttack = false;
            sm.owner.navMesh.isStopped = false;
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
        private void GetBossPattern()
        {
            int pattern = Random.Range(0, 2);
            switch (pattern)
            {
                case 0:
                    sm.ChangeState(sm.trackingState);
                    break;
                case 1:
                    sm.ChangeState(sm.backFlipState);
                    break;
                default:
                    break;
            }
        }

    }
}
