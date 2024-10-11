using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyFSM;
    public class EnemyBehaviorStateMachine : StateMachine
{
    public Enemy owner { get; private set; }
    public DieState dieState { get; private set; }
    public HitState hitState { get; private set; }
    public PatrolState idleState { get; private set; }
    public TrackingState trackingState { get; private set; }
    public ShootArrowState shootArrowState { get; private set; }
    // Start is called before the first frame update
    public EnemyBehaviorStateMachine(Enemy enemy)
    {
        this.owner = enemy;

        dieState = new DieState(this);
        hitState = new HitState(this);
        idleState = new PatrolState(this);
        trackingState = new TrackingState(this);
        shootArrowState = new ShootArrowState(this);
    }
}
