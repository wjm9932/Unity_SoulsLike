using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BT_BossEnemy : Enemy
{
    private CompositeNode root;
    private bool needToResetTree;

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
        BuildBT();

        patrolSpeed = 2f;
        trackingSpeed = 4f;
        trackingStopDistance = 2f;

        viewDistance = 30f;
        _groggyAmount = 0f;
        buffArmorPercent = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
        RecoverGroggy();
    }
    private void FixedUpdate()
    {
    }
    private void LateUpdate()
    {
    }
    protected override void OnEnemyTriggerStay(GameObject target, Collider collider)
    {
        base.OnEnemyTriggerStay(target, collider);
        SoundManager.Instance.Play2DSoundEffect(SoundManager.SoundEffectType.ENEMY_HIT, 0.12f);

        lastTimeDamaged = Time.time;

        if (GetComponent<BehaviorTreeBuilder>().blackboard.GetData<bool>("isGroggy") == false)
        {
            currentGroggyTime = recoverGroggyTime;

            groggyAmount += target.GetComponent<LivingEntity>().damage * 0.5f;

            if (groggyAmount >= maxGroggyAmount)
            {
                groggyAmount = Mathf.Clamp(groggyAmount, 0, maxGroggyAmount);
                GetComponent<BehaviorTreeBuilder>().blackboard.SetData<bool>("isGroggy", true);
            }
        }

    }
    public override void Die()
    {

        base.Die();
    }

    private void RecoverGroggy()
    {
        if (GetComponent<BehaviorTreeBuilder>().blackboard.GetData<bool>("isGroggy") == true)
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

    private bool RandomExecute(float chances)
    {
        return Random.Range(0f, 1f) <= chances;
    }

    private bool IsInRange(float range)
    {
        return Vector3.Distance(GetComponent<BehaviorTreeBuilder>().blackboard.GetData<GameObject>("target").transform.position, transform.position) <= range;
    }
    private bool CheckTargetIsAvaliable()
    {
        if (GetComponent<BehaviorTreeBuilder>().blackboard.GetData<GameObject>("target") == null)
        {
            return false;
        }
        else
        {
            if (GetComponent<BehaviorTreeBuilder>().blackboard.GetData<GameObject>("target").GetComponent<LivingEntity>().isDead == true)
            {
                GetComponent<BehaviorTreeBuilder>().blackboard.SetData<GameObject>("target", null);
                needToResetTree = true;
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    private void BuildBT()
    {
        var builder = GetComponent<BehaviorTreeBuilder>();
        builder.blackboard.InitializeBlackBoard(this.gameObject);
        builder.blackboard.SetData<GameObject>("target", null);
        builder.blackboard.SetData<bool>("isGroggy", false);
        builder.blackboard.SetData<bool>("isAttacking", false);

        needToResetTree = false;

        root = builder
        .AddSelector()
        #region Sequence 1
            .AddSequence()
                .AddCondition(() => isDead == true)
                .AddAction(new Die(builder.blackboard), builder.actionManager)
            .EndComposite()
        #endregion
        #region Sequence 2
            .AddSequence()
                .AddCondition(() => builder.blackboard.GetData<bool>("isGroggy"))
                .AddAction(new Groggy(builder.blackboard), builder.actionManager)
            .EndComposite()
        #endregion
        #region Sequence 3
            .AddSequence()
                .AddCondition(() => CheckTargetIsAvaliable() || builder.blackboard.GetData<bool>("isAttacking") == true)
                .AddAttackSelector()
                    .AddSequence()
                        .AddCondition(() => !IsInRange(7f))
                        .AddAction(new Track(builder.blackboard), builder.actionManager)
                    .EndComposite()
                    .AddAttackSequence()
                        .AddCondition(() => !IsInRange(4f))
                        .AddRandomAttackSelector()
                            .AddAction(new JumpAttack(builder.blackboard), builder.actionManager)
                            .AddAction(new Track(builder.blackboard), builder.actionManager)
                        .EndComposite()
                    .EndComposite()
                    .AddAttackSequence()
                        .AddRandomAttackSelector()
                            .AddAttackSequence()
                                .AddAction(new BackFlip(builder.blackboard), builder.actionManager)
                                .AddRandomAttackSelector()
                                    .AddAction(new JumpAttack(builder.blackboard), builder.actionManager)
                                    .AddAttackSequence()
                                        .AddAction(new StandBy(builder.blackboard), builder.actionManager)
                                        .AddAction(new DashAttack(builder.blackboard), builder.actionManager)
                                        .AddCondition(CheckTargetIsAvaliable)
                                        .AddCondition(() => RandomExecute(0.6f))
                                        .AddAction(new JumpAttack(builder.blackboard), builder.actionManager)
                                    .EndComposite()
                                .EndComposite()
                            .EndComposite()
                            .AddAttackSequence()
                                .AddAction(new Track(builder.blackboard), builder.actionManager)
                                .AddRandomAttackSelector()
                                    .AddAction(new SwordSwingAttack(builder.blackboard), builder.actionManager)
                                    .AddAction(new SwordChargeAttack(builder.blackboard), builder.actionManager)
                                .EndComposite()
                            .EndComposite()
                        .EndComposite()
                        .AddCondition(CheckTargetIsAvaliable)
                        .AddCondition(() => IsInRange(5f) == true)
                        .AddCondition(() => RandomExecute(0.6f))
                        .AddAction(new StabAttack(builder.blackboard), builder.actionManager)
                    .EndComposite()
                .EndComposite()
            .EndComposite()
            #endregion
            .AddSequence()
                .AddCondition(() => needToResetTree ? !(needToResetTree = false) : false)
                .AddAction(new ResetNode(builder.blackboard), builder.actionManager)
            .EndComposite()
            .AddAction(new Patrol(builder.blackboard), builder.actionManager)
        .EndComposite()
        .Build();
    }
}
