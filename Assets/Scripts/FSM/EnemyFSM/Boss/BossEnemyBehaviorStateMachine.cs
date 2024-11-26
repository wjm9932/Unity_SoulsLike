using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BossEnemyFSM;

public class BossEnemyBehaviorStateMachine : StateMachine
{
    public BossEnemy owner { get; }
    public PatrolState patrolState { get; }
    public TrackState trackingState { get; }
    public SwordAttackState swordAttackState { get; }
    public JumpAttackState jumpAttackState { get; }
    public DashAttackState dashAttackState { get; }
    public StanbyStabAttackState stanbyStabAttackState { get; }
    public BackFlipState backFlipState { get; }
    public StabAttackState stabAttackState { get; }
    public ChargingSwordAttackState chargingSwordAttackState { get; }
    public BossEnemyBehaviorStateMachine(BossEnemy enemy)
    {   
        this.owner = enemy;

        patrolState = new PatrolState(this);
        trackingState = new TrackState(this);
        swordAttackState = new SwordAttackState(this);
        jumpAttackState = new JumpAttackState(this);
        dashAttackState = new DashAttackState(this);
        stanbyStabAttackState = new StanbyStabAttackState(this);
        backFlipState = new BackFlipState(this);
        stabAttackState = new StabAttackState(this);
        chargingSwordAttackState = new ChargingSwordAttackState(this);
    }
}
