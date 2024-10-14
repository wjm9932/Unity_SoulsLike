using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{

    public LayerMask whatIsTarget;
    public Transform eyeTransform;
    public GameObject target { get; set; }
    public float viewDistance { get; protected set; }
    public float fieldOfView { get; protected set; }
    public float trackingStopDistance { get; protected set; }

    public NavMeshAgent navMesh { get; private set; }
    public Animator animator { get; private set; }

    [SerializeField]
    protected GameObject[] dropItem;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        canBeDamaged = true;
        isDead = false;
    }
    public void SetNavMeshArea(int mask)
    {
        navMesh.areaMask = 1 << mask;
    }
    protected virtual void OnEnemyTriggerStay(Collider other)
    {
        var hitPoint = other.ClosestPoint(transform.position);
        Vector3 hitNormal = (hitPoint - transform.position).normalized;
        EffectManager.Instance.PlayHitEffect(hitPoint, hitNormal, transform, EffectManager.EffectType.Flesh);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            LivingEntity player = other.transform.root.GetComponent<LivingEntity>();

            if (player != null && player.canAttack == true)
            {
                if (ApplyDamage(player) == true)
                {
                    OnEnemyTriggerStay(other);
                }
            }
        }
    }
    public override void Die()
    {
        base.Die();

        Destroy(this.gameObject, 3f);
        GetComponent<Collider>().enabled = false;
        DropItem();
    }
    protected void DropItem()
    {
        for (int i = 0; i < dropItem.Length; i++)
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
