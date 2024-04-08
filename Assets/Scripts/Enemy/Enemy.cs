using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public NavMeshAgent navMesh { get; private set; }
    public Rigidbody rb { get; private set; }
    public Animator animator { get; private set; }
    public NavMeshPath path { get;  private set; }

    [SerializeField]
    private Character character;
    private EnemyBehaviorStateMachine enemyBehaviorStateMachine;

    private Vector3 currDest;
    private int index = 1;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }
    // Start is called before the first frame update
    void Start()
    {
        navMesh.updatePosition = false;
        //enemyBehaviorStateMachine = new EnemyBehaviorStateMachine(this, character);
        //enemyBehaviorStateMachine.ChangeState(enemyBehaviorStateMachine.idleState);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, GetAngle(), Time.deltaTime * 5);
        GetPath();
        currDest = path.corners[index];
        if (IsArrivedCurrentDestination())
        {
            ++index;
        }
        else
        {
            Vector3 direction = (currDest - transform.position).normalized;
            transform.position += direction * 3f * Time.deltaTime;
        }
        //enemyBehaviorStateMachine.Update();
    }
    private void FixedUpdate()
    {
        //enemyBehaviorStateMachine.PhysicsUpdate();
    }
    private void LateUpdate()
    {
        //enemyBehaviorStateMachine.LateUpdate();
    }
    private void OnAnimationEnterEvent()
    {
        //enemyBehaviorStateMachine.OnAnimationEnterEvent();
    }
    private void OnAnimationExitEvent()
    {
        //enemyBehaviorStateMachine.OnAnimationExitEvent();
    }
    private void OnAnimationTransitionEvent()
    {
        //enemyBehaviorStateMachine.OnAnimationTransitionEvent();
    }

    void GetPath()
    {
        navMesh.nextPosition = transform.position;
        Clear();
        navMesh.CalculatePath(character.transform.position, path);
    }

    bool IsArrivedCurrentDestination()
    {
        if(Vector3.Distance(currDest, transform.position) < 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Clear()
    {
        path.ClearCorners();
        index = 1;
    }

    private Quaternion GetAngle()
    {
        Vector3 direction = (currDest - transform.position).normalized;
        direction.y = 0;

        return Quaternion.LookRotation(direction);
    }
}
