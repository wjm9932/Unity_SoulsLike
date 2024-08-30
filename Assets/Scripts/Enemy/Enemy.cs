using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public NavMeshAgent navMesh { get; private set; }
    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }
    
    public RaycastHit slopeHit;

    [SerializeField]
    private Character character;
    [SerializeField]
    private float maxSlopeAngle;

    public Attack attack { get; private set; }
    private EnemyBehaviorStateMachine enemyBehaviorStateMachine;
    public bool isTest = true;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        attack = GetComponent<Attack>();

        navMesh.updateRotation = false;
        canBeDamaged = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        isTest = true;
        enemyBehaviorStateMachine = new EnemyBehaviorStateMachine(this, character);
        enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.idleState);
    }

    // Update is called once per frame
    void Update()
    {
        enemyBehaviorStateMachine.Update();
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            Attack player = other.transform.root.GetComponent<Attack>();

            if (player.canAttack == true && this.canBeDamaged == true)
            {
                var hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = (transform.position - hitPoint).normalized;
                EffectManager.Instance.PlayHitEffect(hitPoint, hitNormal, other.transform, EffectManager.EffectType.Flesh);
            }
        }
    }
}
