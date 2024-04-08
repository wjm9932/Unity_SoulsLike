using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyFSM;
public class EnemyBehaviorStateMachine : StateMachine
{
    public Enemy enemy { get; }
    public Character character { get; }
    public IdleState idleState { get; }
    public SwordAttackState SwordAttackState { get; }
    public EnemyBehaviorStateMachine(Enemy enemy, Character character)
    {   
        this.enemy = enemy;
        this.character = character;

        idleState = new IdleState(this);
        SwordAttackState = new SwordAttackState(this);
    }
}
