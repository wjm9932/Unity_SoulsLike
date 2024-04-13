using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyFSM;
public class EnemyBehaviorStateMachine : StateMachine
{
    public Enemy enemy { get; }
    public Character character { get; }
    public IdleState idleState { get; }
    public SwordAttackState swordAttackState { get; }
    public JumpAttackState jumpAttackState { get; }
    public DashAttackState dashAttackState { get; }
    public BackFlipState backFlipState { get; }
    public EnemyBehaviorStateMachine(Enemy enemy, Character character)
    {   
        this.enemy = enemy;
        this.character = character;

        idleState = new IdleState(this);
        swordAttackState = new SwordAttackState(this);
        jumpAttackState = new JumpAttackState(this);
        dashAttackState = new DashAttackState(this);
        backFlipState = new BackFlipState(this);
    }
}