using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossEnemyFSM
{
    public class StanbyStabAttackState : BossEnemyPatternState
    {
        private bool isReadyToAttack;
        public StanbyStabAttackState(BossEnemyBehaviorStateMachine sm) : base(sm)
        {
            stoppingDistance = 0f;
        }

        public override void Enter()
        {
            sm.owner.StartCoroutine(GetReady());

            isReadyToAttack = false;
            sm.owner.navMesh.isStopped = true;
            sm.owner.animator.SetBool ("isStabReady", true);
        }
        public override void Update()
        {
            if (isReadyToAttack == false && Vector3.Distance(sm.owner.transform.position, sm.owner.target.transform.position) >= 3f)
            {
                sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, GetLookAtAngle(), Time.deltaTime * 10);
            }
            else
            {
                sm.ChangeState(sm.dashAttackState);
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
            sm.owner.animator.SetBool("isStabReady", false);
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
            sm.owner.animator.SetFloat("HandWeight", 0f);
        }
        IEnumerator GetReady()
        {
            yield return new WaitForSeconds(1f);
            isReadyToAttack = true;
        }
    }
}
