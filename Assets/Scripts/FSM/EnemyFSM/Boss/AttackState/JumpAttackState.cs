using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossEnemyFSM
{
    public class JumpAttackState : BossEnemyPatternState
    {

        private float distance;
        public JumpAttackState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 2f;
        }

        public override void Enter()
        {
            dir = GetLookAtAngle();

            distance = Vector3.Distance(sm.owner.transform.position, sm.owner.target.transform.position);
            agentSpeed = (distance - stoppingDistance) / 1f;

            sm.owner.navMesh.speed = agentSpeed;
            sm.owner.navMesh.stoppingDistance = stoppingDistance;
            sm.owner.navMesh.SetDestination(sm.owner.target.transform.position);
            sm.owner.animator.SetBool("isJumpAttack", true);
            sm.owner.SetDamage(30f);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 10);

            if (sm.owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f && sm.owner.animator.IsInTransition(0) == false)
            {
                sm.ChangeState(sm.trackingState);

                //sm.ChangeState(sm.idleState);
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
            sm.owner.animator.SetBool("isJumpAttack", false);
        }
        public override void OnAnimationEnterEvent()
        {
            var camera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
            camera.GetComponent<CameraShake>().ShakeCamera();
            SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.JUMP_ATTACK, 0.6f, sm.owner.transform.position, Quaternion.identity, sm.owner.gameObject.transform);
        }
        public override void OnAnimationExitEvent()
        {

        }
        public override void OnAnimationTransitionEvent()
        {
            SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_JUMP, 0.5f, sm.owner.transform.position, Quaternion.identity, sm.owner.gameObject.transform);
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
                default:
                    break;
            }
        }
    }
}
