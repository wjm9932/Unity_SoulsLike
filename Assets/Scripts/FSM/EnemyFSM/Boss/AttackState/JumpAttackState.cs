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
            sm.owner.animator.SetTrigger("Jump Attack");
            sm.owner.SetDamage(20f);
        }
        public override void Update()
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 10);

            if (sm.owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f && sm.owner.animator.IsInTransition(0) == false)
            {
                GetBossPattern();

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

        }
        public override void OnAnimationEnterEvent()
        {
            var camera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
            camera.GetComponent<CameraShake>().ShakeCamera();
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
