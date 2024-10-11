using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class NormalEnemy : Enemy
{
    public LayerMask whatIsTarget;
    public Transform eyeTransform;
    public GameObject target { get; set; }
    public float viewDistance { get; private set; }
    public float fieldOfView { get; private set; }

    [SerializeField]
    private GameObject[] dropItem;
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
        animator.SetTrigger("Die");

        Destroy(this.gameObject, 3f);
        GetComponent<Collider>().enabled = false;
        DropItem();
    }

    private void DropItem()
    {
        for(int i = 0; i < dropItem.Length; i++)
        {
            UX.Item item = dropItem[i].GetComponent<UX.Item>();

            if (IsDrop(item.dropChance) == true)
            {
                Instantiate(dropItem[i], this.gameObject.transform.position, Quaternion.identity);
            }
        }
    }

    private bool IsDrop(float chances)
    {
        return Random.Range(0f, 100f) <= chances;
    }
}
