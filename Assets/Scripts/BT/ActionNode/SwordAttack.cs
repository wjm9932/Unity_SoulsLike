using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordAttack : IAction
{
    private Blackboard blackboard;
    private Quaternion dir;
    public SwordAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.SetData<bool>("isAttacking", true);

        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().SetDamage(10f);
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().animator.SetBool("IsSwordAttack", true);
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().isStopped = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<NavMeshAgent>().avoidancePriority = 50;

        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationComplete += FinishAttackAnim;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationTransition += PlaySwordAttackSFX;


        dir = GetLookAtAngle();
    }

    public NodeState Execute()
    {
        blackboard.GetData<GameObject>("Owner").transform.rotation = Quaternion.Slerp(blackboard.GetData<GameObject>("Owner").transform.rotation, dir, Time.deltaTime * 10);

        if (blackboard.GetData<bool>("isAttacking") == true)
        {
            return NodeState.Running;
        }
        else
        {

            return NodeState.Success;
        }

    }

    public void OnExit()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationTransition -= PlaySwordAttackSFX;
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationComplete -= FinishAttackAnim;

        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().animator.SetBool("IsSwordAttack", false);
        blackboard.SetData<bool>("isAttacking", false);
    }

    private void FinishAttackAnim()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().animator.SetBool("IsSwordAttack", false);
        blackboard.SetData<bool>("isAttacking", false);

    }

    private void PlaySwordAttackSFX()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().attackSound = SoundManager.Instance.Play3DSoundEffect(SoundManager.SoundEffectType.WARRIOR_ENEMY_ATTACK,
                0.15f, blackboard.GetData<GameObject>("Owner").transform.position, Quaternion.identity, blackboard.GetData<GameObject>("Owner").transform);
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().attackSound.GetComponent<SoundObjectPool>().removeAction += () => { blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().attackSound = null; };
    }

    private Quaternion GetLookAtAngle()
    {
        Vector3 dir = blackboard.GetData<GameObject>("target").transform.position - blackboard.GetData<GameObject>("Owner").transform.position;
        dir.y = 0;

        return Quaternion.LookRotation(dir);
    }
}
