using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public Transform leftHandPos;
    private float lastTimeDamaged;
    private const float minTimeBetDamaged = 0.5f;

    private const float maxGroggyAmount = 50f;
    private const float recoverGroggyTime = 2f;
    private float _groggyAmount;
    public float groggyAmount
    {
        get { return _groggyAmount; }
        set
        {
            _groggyAmount = value;
            groggyBar.value = _groggyAmount / maxGroggyAmount;
        }
    }
    private float currentGroggyTime = 0f;
    [SerializeField] Slider groggyBar;

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
        _groggyAmount = 0f;
        buffArmorPercent = 0.3f;
        enemyBehaviorStateMachine = new BossEnemyBehaviorStateMachine(this);
        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        enemyBehaviorStateMachine.Update();

        RecoverGroggy();
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
    private void OnAnimatorIK()
    {
        enemyBehaviorStateMachine.OnAnimatorIK();
    }
    protected override void OnEnemyTriggerStay(GameObject target, Collider collider)
    {
        base.OnEnemyTriggerStay(target, collider);
        SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ENEMY_HIT, 0.12f);

        if (enemyBehaviorStateMachine.currentState != enemyBehaviorStateMachine.dieState)
        {
            lastTimeDamaged = Time.time;

            if (enemyBehaviorStateMachine.currentState != enemyBehaviorStateMachine.groggyState)
            {
                currentGroggyTime = recoverGroggyTime;

                groggyAmount += target.GetComponent<LivingEntity>().damage * 0.5f;

                if (groggyAmount >= maxGroggyAmount)
                {
                    groggyAmount = Mathf.Clamp(groggyAmount, 0, maxGroggyAmount);
                    enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.groggyState);
                }
            }
        }
    }
    public override void Die()
    {
        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.dieState);
        base.Die();
    }

    private void RecoverGroggy()
    {
        if(enemyBehaviorStateMachine.currentState == enemyBehaviorStateMachine.groggyState)
        {
            return;
        }

        if (currentGroggyTime > 0f)
        {
            currentGroggyTime -= Time.deltaTime;
        }
        else
        {
            if (groggyAmount > 0f)
            {
                groggyAmount -= 5f * Time.deltaTime;
                groggyAmount = Mathf.Clamp(groggyAmount, 0, maxGroggyAmount);
            }
        }
    }
}
