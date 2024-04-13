using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFSM
{
    public class JumpAttackState : EnemyPatternState
    {
        
        private float distance;
        public JumpAttackState(EnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 2f;
        }

        public override void Enter()
        {
            dir = GetLookAtAngle();
            
            distance = Vector3.Distance(sm.enemy.transform.position, sm.character.transform.position);
            agentSpeed = (distance - stoppingDistance) / 1f;

            sm.enemy.navMesh.speed = agentSpeed;
            sm.enemy.navMesh.stoppingDistance = stoppingDistance;
            sm.enemy.navMesh.SetDestination(sm.character.transform.position);
            sm.enemy.animator.SetTrigger("Jump Attack");
        }
        public override void Update()
        {
            sm.enemy.transform.rotation = Quaternion.Slerp(sm.enemy.transform.rotation, dir, Time.deltaTime * 10);

            if (sm.enemy.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f && sm.enemy.animator.IsInTransition(0) == false)
            {
                //sm.ChangeState(sm.idleState);
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

        }
        public override void OnAnimationEnterEvent()
        {

        }
        public override void OnAnimationExitEvent()
        {

        }
        public override void OnAnimationTransitionEvent()
        {
            if (canAttack == false)
            {
                canAttack = true;
                sm.enemy.swordAttack.SetCanAttack(canAttack);
            }
            else
            {
                canAttack = false;
                sm.enemy.swordAttack.SetCanAttack(canAttack);
            }
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
                    sm.ChangeState(sm.idleState);
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
