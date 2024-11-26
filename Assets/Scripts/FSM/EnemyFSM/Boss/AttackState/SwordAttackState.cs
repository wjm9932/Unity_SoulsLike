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
            sm.owner.animator.SetBool("isAttack", true);
            sm.owner.SetDamage(20f);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 10);

            if (sm.owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f && sm.owner.animator.IsInTransition(0) == false)
            {
                if (IsTargetDead() == false)
                {
                    GetBossPattern();
                }
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
            sm.owner.animator.SetBool("isAttack", false);
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
            SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_SWORD_ATTACK, 0.35f, sm.owner.transform.position, Quaternion.identity, sm.owner.gameObject.transform);
        }
        public override void OnAnimatorIK()
        {
            sm.owner.animator.SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
            sm.owner.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, sm.owner.animator.GetFloat("HandWeight"));
            sm.owner.animator.SetIKPosition(AvatarIKGoal.LeftHand, sm.owner.leftHandPos.position);
        }
        private void GetBossPattern()
        {
            int pattern = Random.Range(0, 5);

            switch (pattern)
            {
                case 0:
                    sm.ChangeState(sm.chargingSwordAttackState);
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
