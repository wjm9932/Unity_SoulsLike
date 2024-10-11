using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NormalEnemyFSM;
    public class EnemyBehaviorStateMachine : StateMachine
{
    public Enemy owner { get; private set; }
    public DieState dieState { get; private set; }
    public IdleState idleState { get; private set; }
    public TrackingState trackingState { get; private set; }
    public ShootArrowState shootArrowState { get; private set; }
    // Start is called before the first frame update
    public EnemyBehaviorStateMachine(Enemy enemy)
    {
        this.owner = enemy;

        dieState = new DieState(this);
        idleState = new IdleState(this);
        trackingState = new TrackingState(this);
        shootArrowState = new ShootArrowState(this);
    }
}
