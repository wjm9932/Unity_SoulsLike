using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class WarriorEnemy : Enemy
{

    private EnemyBehaviorStateMachine enemyBehaviorStateMachine;



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
        entityType = EntityType.ENEMY;
    }
    // Start is called before the first frame update
    void Start()
    {
        health = 10f;
        fieldOfView = 50f;
        viewDistance = 3f;
        enemyBehaviorStateMachine = new EnemyBehaviorStateMachine(this);
        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.idleState);
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



    public override void Die()
    {
        base.Die();

        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.dieState);
    }
}