using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankEnemy : Enemy
{

    private EnemyBehaviorStateMachine enemyBehaviorStateMachine;

    public override float health
    {
        protected set
        {
            base.health = value;
            hpBar.UpdateHealthBar(health, maxHealth);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var leftEyeRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        var leftRayDirection = leftEyeRotation * transform.forward;
        Handles.color = new Color(1f, 1f, 1f, 0.2f);
        Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
    }
#endif

    protected override void Awake()
    {
        base.Awake();
        navMesh.updateRotation = false;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        buffArmorPercent = 0.3f;
        patrolSpeed = 0.75f;
        trackingSpeed = 1.5f;
        trackingStopDistance = 1f;

        health = maxHealth;
        fieldOfView = 100f;
        viewDistance = 5f;
        enemyBehaviorStateMachine = new EnemyBehaviorStateMachine(this);
        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        enemyBehaviorStateMachine.Update();
    }
    private void FixedUpdate()
    {
        enemyBehaviorStateMachine.PhysicsUpdate();
    }
    private void LateUpdate()
    {
        enemyBehaviorStateMachine.LateUpdate();
    }
    private void OnAnimationEnterEvent()
    {
        enemyBehaviorStateMachine.OnAnimationEnterEvent();
    }
    private void OnAnimationExitEvent()
    {
        enemyBehaviorStateMachine.OnAnimationExitEvent();
    }
    private void OnAnimationTransitionEvent()
    {
        enemyBehaviorStateMachine.OnAnimationTransitionEvent();
    }

    protected override void OnEnemyTriggerStay(GameObject target, Collider collider)
    {
        base.OnEnemyTriggerStay(target, collider);

        if (enemyBehaviorStateMachine.currentState != enemyBehaviorStateMachine.dieState)
        {
            this.target = target;
            enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.hitState);
        }
    }
    public override void Die()
    {
        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.dieState);

        base.Die();
    }
}
