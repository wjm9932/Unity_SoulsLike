using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Die : IAction
{
    private Blackboard blackboard;
    public Die(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.GetData<GameObject>("Owner").GetComponent<Enemy>().canAttack = false;
        blackboard.GetData<GameObject>("Owner").GetComponent<Animator>().SetTrigger("Die");

        SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ENEMY_DIE, 0.2f);
        SoundManager.Instance.ChangeBackGroundMusic(AreaType.DUNGEON);
    }
    public NodeState Execute()
    {
        return NodeState.Running;
    }

    public void OnExit()
    {
    }
}
