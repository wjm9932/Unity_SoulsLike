using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : Enemy
{
    public override float health
    {
        protected set
        {
            base.health = value;
            hpBar.UpdateHealthBar(health, maxHealth);
        }
    }

    public override bool canBeDamaged
    {
        get
        {
            return Time.time >= lastTimeDamaged + minTimeBetDamaged ? true : false;
        }
    }

    private float lastTimeDamaged;
    private const float minTimeBetDamaged = 0.5f;

    private BossEnemyBehaviorStateMachine enemyBehaviorStateMachine;
    protected override void Awake()
    {
        base.Awake();

        health = maxHealth;

        navMesh.updateRotation = false;
        canBeDamaged = true;
        isDead = false;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        buffArmorPercent = 0.3f;
        enemyBehaviorStateMachine = new BossEnemyBehaviorStateMachine(this);
        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.stabAttackState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.swordAttackState);
        }

        //enemyBehaviorStateMachine.Update();


        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    isTest = !isTest;
        //}

        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.idleState);
        //}
        //else if(Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.swordAttackState);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.stabAttackState);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.backFlipState);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.jumpAttackState);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.stanbyStabAttackState);
        //}
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

        lastTimeDamaged = Time.time;
    }
    public override void Die()
    {
        base.Die();
    }
}
