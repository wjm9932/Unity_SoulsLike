using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossEnemyFSM
{
    public class ChargingSwordAttackState : BossEnemyPatternState
    {
        public ChargingSwordAttackState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {

        }

        public override void Enter()
        {
            dir = GetLookAtAngle();
            sm.owner.navMesh.isStopped = true;
            sm.owner.animator.SetBool("isUpAttack", true);
            sm.owner.SetDamage(30f);
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
            sm.owner.animator.SetBool("isUpAttack", false);
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
            var camera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
            camera.GetComponent<CameraShake>().ShakeCamera();
            SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_CHARGE_ATTACK, 0.6f, sm.owner.transform.position, Quaternion.identity, sm.owner.gameObject.transform);
            EffectManager.Instance.PlayEffect(sm.owner.transform.position + sm.owner.transform.forward * 2f, Vector3.up, sm.owner.gameObject.transform, ObjectPoolManager.ObjectType.DUST);
        }
        public override void OnAnimatorIK()
        {
            sm.owner.animator.SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
            sm.owner.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, sm.owner.animator.GetFloat("HandWeight"));
            sm.owner.animator.SetIKPosition(AvatarIKGoal.LeftHand, sm.owner.leftHandPos.position);
        }
        private void GetBossPattern()
        {
            int pattern = Random.Range(0, 2);

            switch (pattern)
            {
                case 0:
                    sm.ChangeState(sm.stabAttackState);
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