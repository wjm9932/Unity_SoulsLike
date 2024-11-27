using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace BossEnemyFSM
{
    public class GroggyState : IState
    {
        protected BossEnemyBehaviorStateMachine sm;
        private float groggyTime;
        private bool isDone;
        public GroggyState(BossEnemyBehaviorStateMachine sm)
        {
            this.sm = sm;
            groggyTime = 1f;
        }

        public virtual void Enter()
        {
            isDone = false;
            sm.owner.canAttack = false;
            sm.owner.animator.SetTrigger("isGroggy");

        }
        public virtual void Update()
        {
            if (sm.owner.canAttack == true)
            {
                StackTrace stackTrace = new StackTrace(true);
                UnityEngine.Debug.Log(stackTrace.ToString());
                UnityEngine.Debug.LogError(sm.owner.name + "is in Die state: " + sm.owner.canAttack);
            }
            if(isDone == true)
            {
                sm.ChangeState(sm.trackingState);
            }
        }
        public virtual void PhysicsUpdate()
        {

        }
        public virtual void LateUpdate()
        {

        }
        public virtual void Exit()
        {
            sm.owner.animator.SetTrigger("isGroggyFinished");
        }
        public virtual void OnAnimationEnterEvent()
        {
            sm.owner.StartCoroutine(TriggerEndGroggy());
        }
        public virtual void OnAnimationExitEvent()
        {
            isDone = true;
        }
        public virtual void OnAnimationTransitionEvent()
        {

        }
        public virtual void OnAnimatorIK()
        {
            sm.owner.animator.SetFloat("HandWeight", 0f);
        }

        IEnumerator TriggerEndGroggy()
        {
            yield return new WaitForSeconds(groggyTime);
            sm.owner.animator.SetTrigger("GroggyEnd");
        }
    }
}

