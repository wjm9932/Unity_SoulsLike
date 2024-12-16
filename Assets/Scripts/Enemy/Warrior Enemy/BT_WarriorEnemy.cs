using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ObjectChangeEventStream;

public class BT_WarriorEnemy : Enemy
{
    private Blackboard blackboard;
    private Selector root;
    private ActionManager actionManager;

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
    private void OnAnimationEnterEvent()
    {
    }
    private void OnAnimationExitEvent()
    {
    }
    private void OnAnimationTransitionEvent()
    {
    }

    protected override void OnEnemyTriggerStay(GameObject target, Collider collider)
    {
        base.OnEnemyTriggerStay(target, collider);
    }
    public override void Die()
    {
        base.Die();
    }

    private void BuildBT()
    {
        blackboard = new Blackboard();
        actionManager = new ActionManager();

        blackboard.InitializeBlackBoard(this.gameObject);
        blackboard.SetData<bool>("isTargetOnSight", false);
        blackboard.SetData<bool>("isInAttakRange", false);
        blackboard.SetData<GameObject>("target", null);

        root = new Selector();

        var attackSequence = new Sequence();
        attackSequence.AddChild(new ConditionNode(() => blackboard.GetData<bool>("isInAttackRange")));
        attackSequence.AddChild(new ActionNode(new Track(blackboard), actionManager));
        root.AddChild(attackSequence);

        var trackSequence = new Sequence();
        trackSequence.AddChild(new ConditionNode(() => blackboard.GetData<bool>("isTargetOnSight")));
        trackSequence.AddChild(new ActionNode(new Track(blackboard), actionManager));
        root.AddChild(trackSequence);

        root.AddChild(new ActionNode(new Patrol(blackboard), actionManager));
    }

    private bool IsInAttackRange()
    {
        return GetComponent<NavMeshAgent>().remainingDistance <= GetComponent<NavMeshAgent>().stoppingDistance;
    }
}
