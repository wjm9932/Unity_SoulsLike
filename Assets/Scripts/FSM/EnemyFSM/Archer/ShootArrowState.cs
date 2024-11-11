using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrowState : EnemyPatternState
{
    private bool isShoot;
    public ShootArrowState(EnemyBehaviorStateMachine sm) : base(sm)
    {

    }
    public override void Enter()
    {
        isShoot = false;
        sm.owner.navMesh.isStopped = true;
        sm.owner.SetDamage(10f);
        sm.owner.StartCoroutine(DelayForAnimation());
        sm.owner.navMesh.avoidancePriority = 50;
    }
    IEnumerator DelayForAnimation()
    {
        yield return new WaitForEndOfFrame(); // the reason why I do this is that if I dont do this, the sm.owner.animator.SetBool("IsShotArrow", false); is never set to false because isShotArrow true->false->true in one frame
        sm.owner.animator.SetBool("IsShotArrow", true);
    }
    public override void Update()
    {
        if (isShoot == true)
        {
            Vector3 directionToTarget = sm.owner.target.transform.position - sm.owner.GetComponent<ArcherEnemy>().arrowPosition.position;
            directionToTarget.y = 0; 
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            Arrow arrow = ObjectPoolManager.Instance.GetPoolableObject(ObjectPoolManager.ObjectType.ARROW).GetComponent<Arrow>();
            arrow.Initialize(sm.owner.GetComponent<ArcherEnemy>().arrowPosition.position, targetRotation, sm.owner.transform);
            SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.SHOT_ARROW, 0.4f, sm.owner.transform.position, Quaternion.identity, sm.owner.transform);
            sm.ChangeState(sm.trackingState);
        }
        else
        {
            sm.owner.transform.rotation = Quaternion.Slerp(sm.owner.transform.rotation, GetLookAtAngle(), Time.deltaTime * 10);
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
        isShoot = false;
        sm.owner.animator.SetBool("IsShotArrow", false);
    }
    public override void OnAnimationEnterEvent()
    {
        sm.owner.attackSound = SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.ARCHER_ENEMY_ATTACK, 
            1f, sm.owner.transform.position, Quaternion.identity, sm.owner.transform);
        sm.owner.attackSound.GetComponent<SoundObjectPool>().removeAction += () => { sm.owner.attackSound = null; };
    }
    public override void OnAnimationExitEvent()
    {

    }
    public override void OnAnimationTransitionEvent()
    {
        isShoot = true;
    }
    public override void OnAnimatorIK()
    {

    }
}
