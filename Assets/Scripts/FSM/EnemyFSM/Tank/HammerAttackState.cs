using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttackState : EnemyPatternState
{
    Quaternion dir;
    private bool isDone;
    public HammerAttackState(EnemyBehaviorStateMachine sm) : base(sm)
    {

    }

    public override void Enter()
    {
        isDone = false;
        dir = GetLookAtAngle();
        sm.owner.navMesh.isStopped = true;
        sm.owner.SetDamage(20f);
        sm.owner.StartCoroutine(DelayForAnimation());
        sm.owner.navMesh.avoidancePriority = 50;
    }
    IEnumerator DelayForAnimation()
    {
        yield return new WaitForEndOfFrame(); // the reason why I do this is that if I dont do this, the sm.owner.animator.SetBool("IsShotArrow", false); is never set to false because isShotArrow true->false->true in one frame
        sm.owner.animator.SetBool("IsHammerAttack", true);
    }
    public override void Update()
    {
        sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, dir, Time.deltaTime * 10);

        if (isDone == true)
        {
            sm.ChangeState(sm.trackingState);
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
        sm.owner.animator.SetBool("IsHammerAttack", false);
    }
    public override void OnAnimationEnterEvent()
    {
    }
    public override void OnAnimationExitEvent()
    {
        isDone = true;
    }
    public override void OnAnimationTransitionEvent()
    {
        sm.owner.attackSound = SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.WARRIOR_ENEMY_ATTACK, 
            0.15f, sm.owner.transform.position, Quaternion.identity, sm.owner.transform);
        sm.owner.attackSound.GetComponent<SoundObjectPool>().removeAction += () => { sm.owner.attackSound = null; };
    }
    public override void OnAnimatorIK()
    {

    }
}