using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossEnemyFSM
{
    public class SwordAttackState : BossEnemyPatternState
    {
        public SwordAttackState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            
        }

        public override void Enter()
        {
            dir = GetLookAtAngle();
            sm.owner.navMesh.isStopped = true;
            sm.owner.animator.SetTrigger("Attack");
            sm.owner.SetDamage(10f);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 10);

            if (sm.owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f && sm.owner.animator.IsInTransition(0) == false)
            {
                GetBossPattern();
                //sm.ChangeState(sm.idleState);
                //sm.ChangeState(sm.stabAttackState);
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
            int pattern = Random.Range(0, 5);
            
            switch (pattern)
            {
                case 0:
                    sm.ChangeState(sm.idleState);
                    break;
                case 1:
                    sm.ChangeState(sm.stabAttackState);
                    break;
                case 2:
                    sm.ChangeState(sm.stabAttackState);
                    break;
                case 3:
                    sm.ChangeState(sm.jumpAttackState);
                    break;
                case 4:
                    sm.ChangeState(sm.backFlipState);
                    break;
                default:
                    break;
            }
        }
        
    }
}
