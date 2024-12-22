using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordChargeAttack : BossAttackAction
{
    private Quaternion dir;
    public SwordChargeAttack(Blackboard blackBoard) : base(blackBoard)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        dir = GetLookAtAngle();

        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().SetDamage(40f);
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isUpAttack", true);
    }

    public override NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, dir, Time.deltaTime * 10);

        return base.Execute();
    }

    public override void OnExit()
    {
        base.OnExit();
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isUpAttack", false);
    }

    public override void OnAnimationEnter()
    {
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_START_CHARGING, 0.6f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
    }

    public override void OnAnimationTransition()
    {
        var camera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        camera.GetComponent<CameraShake>().ShakeCamera();
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_CHARGE_ATTACK, 0.3f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
        EffectManager.Instance.PlayEffect(blackboard.GetData<GameObject>("Owner").transform.position + blackboard.GetData<GameObject>("Owner").transform.forward * 2f, Vector3.up, blackboard.GetData<GameObject>("Owner").transform, ObjectPoolManager.ObjectType.DUST);
    }

    public override void OnAnimatorIK()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetIKPositionWeight(AvatarIKGoal.LeftHand, blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().GetFloat("HandWeight"));
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetIKPosition(AvatarIKGoal.LeftHand, blackboard.GetData<GameObject>("Owner").GetComponent<BT_BossEnemy>().leftHandPos.position);
    }
}