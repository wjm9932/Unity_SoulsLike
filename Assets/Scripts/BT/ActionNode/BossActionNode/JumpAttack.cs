using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumpAttack : BossAttackAction
{
    private Quaternion dir;
    public JumpAttack(Blackboard blackBoard) : base(blackBoard)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = false;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance = 2f;

        dir = GetLookAtAngle();
        SetDashDestinationAndSpeed();

        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isJumpAttack", true);
    }

    public override NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, dir, Time.deltaTime * 10);

        return base.Execute();    }

    public override void OnExit()
    {
        base.OnExit();
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetBool("isJumpAttack", false);
    }
   
    public override void OnAnimationEnter()
    {
        var camera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        camera.GetComponent<CameraShake>().ShakeCamera();
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.JUMP_ATTACK, 0.6f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
        EffectManager.Instance.PlayEffect(blackboard.GetData<GameObject>("Owner").transform.position + blackboard.GetData<GameObject>("Owner").transform.forward * 2f, Vector3.up, blackboard.GetData<GameObject>("Owner").transform, ObjectPoolManager.ObjectType.DUST);
    }
    public override void OnAnimationTransition()
    {
        SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.BOSS_JUMP, 0.5f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
    }

    public override void OnAnimationExit()
    {

    }
    public override void OnAnimatorIK()
    {
        //blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 1, 0.1f, Time.deltaTime * 0.1f);
        //blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetIKPositionWeight(AvatarIKGoal.LeftHand, blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().GetFloat("HandWeight"));
        //blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetIKPosition(AvatarIKGoal.LeftHand, blackboard.GetData<GameObject>("Owner").GetComponent<BT_BossEnemy>().leftHandPos.position);

        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetFloat("HandWeight", 0f);
    }

    private void SetDashDestinationAndSpeed()
    {
        float distance = Vector3.Distance(blackboard.GetData<GameObject>("Owner").transform.position, blackboard.GetData<GameObject>("target").transform.position);
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().SetDestination(blackboard.GetData<GameObject>("target").transform.position);

        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().speed = Mathf.Max(0, (distance - blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().stoppingDistance) / 1f);
    }
    private Quaternion GetLookAtAngle()
    {
        Vector3 dir = blackboard.GetData<GameObject>("target").transform.position - blackboard.GetData<GameObject>("Owner").transform.position;
        dir.y = 0;

        return Quaternion.LookRotation(dir);
    }
}
