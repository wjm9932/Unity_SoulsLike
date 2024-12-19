using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hit : IAction, ICompositionNodeResettable
{
    private Blackboard blackboard;
    private Action onResetCompositionNode;
    public Hit(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        onResetCompositionNode();

        blackboard.SetData<bool>("isHit", true);

        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationComplete += FinishHitAnim;

        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().canAttack = false;
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().canBeDamaged = false;
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().navMesh.isStopped = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().animator.SetBool("IsHit", true);

        if (blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().attackSound != null)
        {
            blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().attackSound.GetComponent<AudioSource>().Stop();
        }
        SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ENEMY_HIT, 0.12f);

    }

    public NodeState Execute()
    {
        if (blackboard.GetData<bool>("isHit") == true)
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
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().canBeDamaged = true;
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().animator.SetBool("IsHit", false);
        blackboard.GetData<GameObject>("Owner").GetComponent<AnimationEventHandler>().onAnimationComplete -= FinishHitAnim;
        blackboard.SetData<bool>("isHit", false);
    }

    private void FinishHitAnim()
    {
        blackboard.SetData<bool>("isHit", false);
    }

   public void SetResetAction(Action resetAction)
    {
        this.onResetCompositionNode = resetAction;
    }
}
