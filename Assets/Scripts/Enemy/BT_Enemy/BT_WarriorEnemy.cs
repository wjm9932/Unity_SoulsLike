using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BT_WarriorEnemy : Enemy
{
    private CompositeNode root;
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

        BuildBT();

        buffArmorPercent = 0f;

        patrolSpeed = 1f;
        trackingSpeed = 2f;
        trackingStopDistance = 1f;

        health = maxHealth;
        fieldOfView = 100f;
        viewDistance = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
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
        GetComponent<BehaviorTreeBuilder>().blackboard.SetData<bool>("isHit", true);
        GetComponent<BehaviorTreeBuilder>().blackboard.SetData<GameObject>("target", target);

    }
    public override void Die()
    {
        base.Die();
    }

    private void BuildBT()
    {
        var builder = GetComponent<BehaviorTreeBuilder>();
        builder.blackboard.InitializeBlackBoard(this.gameObject);
        builder.blackboard.SetData<GameObject>("target", null);
        builder.blackboard.SetData<bool>("isAttacking", false);
        builder.blackboard.SetData<bool>("isHit", false);

        root = builder
        .AddSelector()
            .AddSequence()
                .AddCondition(() => isDead == true)
                .AddAction(new Die(builder.blackboard), builder.actionManager)
            .EndComposite()
            .AddSequence()
                .AddCondition(() => builder.blackboard.GetData<bool>("isHit"))
                .AddAction(new Hit(builder.blackboard), builder.actionManager)
            .EndComposite()
            .AddSequence()
                .AddCondition(() => builder.blackboard.GetData<GameObject>("target") != null || builder.blackboard.GetData<bool>("isAttacking"))
                .AddSelector()
                    .AddAttackSelector()
                        .AddSequence()
                            .AddCondition(() => builder.blackboard.GetData<bool>("isAttacking") || IsInRange())
                            .AddAction(new SwordAttack(builder.blackboard), builder.actionManager)
                        .EndComposite()
                    .EndComposite()
                    .AddAction(new Track(builder.blackboard), builder.actionManager)
                .EndComposite()
            .EndComposite()
            .AddAction(new Patrol(builder.blackboard), builder.actionManager)
        .EndComposite()
        .Build();

    }

    private IEnumerator CheckIsInRange()
    {
        yield return null;
        while (true)
        {
            if (GetComponent<BehaviorTreeBuilder>().blackboard.GetData<bool>("isAttacking") == false && GetComponent<BehaviorTreeBuilder>().blackboard.GetData<GameObject>("target") != null)
            {
                if (Vector3.Distance(GetComponent<BehaviorTreeBuilder>().blackboard.GetData<GameObject>("target").transform.position, transform.position) <= 1f)
                {
                    GetComponent<BehaviorTreeBuilder>().blackboard.SetData<bool>("isAttacking", true);
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
    private bool IsInRange()
    {
        if (Vector3.Distance(GetComponent<BehaviorTreeBuilder>().blackboard.GetData<GameObject>("target").transform.position, transform.position) <= trackingStopDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
