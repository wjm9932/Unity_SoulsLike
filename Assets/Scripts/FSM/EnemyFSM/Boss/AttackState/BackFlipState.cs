using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BossEnemyFSM
{
    public class BackFlipState : BossEnemyPatternState
    {
        private float distance;
        public BackFlipState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            sm.owner.navMesh.stoppingDistance = stoppingDistance;

            dir = GetLookAtAngle();
            SetDashDestinationAndSpeed();
            sm.owner.animator.SetBool("isBackFlip", true);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 30);

            if (sm.owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f && sm.owner.animator.IsInTransition(0) == false)
            {
                GetBossPattern();
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
            sm.owner.animator.SetBool("isBackFlip", false);
        }
        public override void OnAnimationEnterEvent()
        {
            SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_JUMP, 0.5f, sm.owner.transform.position, Quaternion.identity, sm.owner.gameObject.transform);
        }
        public override void OnAnimationExitEvent()
        {

        }
        public override void OnAnimationTransitionEvent()
        {
            SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_FLIP, 0.35f, sm.owner.transform.position, Quaternion.identity, sm.owner.gameObject.transform);
            EffectManager.Instance.PlayEffect(sm.owner.transform.position, Vector3.up, sm.owner.gameObject.transform, ObjectPoolManager.ObjectType.DUST);

        }
        public override void OnAnimatorIK()
        {
            sm.owner.animator.SetFloat("HandWeight", 0f);
        }
        private void GetBossPattern()
        {
            int pattern = Random.Range(0, 2);
            switch (pattern)
            {
                case 0:
                    sm.ChangeState(sm.stanbyStabAttackState);
                    break;
                case 1:
                    sm.ChangeState(sm.jumpAttackState);
                    break;
                default:
                    break;
            }
        }

        private void SetDashDestinationAndSpeed()
        {
            Vector3 forwardDirection = dir * Vector3.forward;
            Vector3 backOffset = forwardDirection * -5;
            Vector3 dashDestination = sm.owner.transform.position + backOffset;

            sm.owner.navMesh.SetDestination(dashDestination);

            distance = Vector3.Distance(sm.owner.transform.position, dashDestination);
            agentSpeed = distance / 0.75f;
            sm.owner.navMesh.speed = agentSpeed;
        }
    }
}
