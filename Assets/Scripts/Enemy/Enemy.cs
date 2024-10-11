using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{

    [SerializeField]
    protected GameObject[] dropItem;

    public NavMeshAgent navMesh { get; private set; }
    public Animator animator { get; private set; }

    public override bool canBeDamaged
    {
        get
        {
            return Time.time >= lastTimeDamaged + minTimeBetDamaged ? true : false;
        }
    }

    private float lastTimeDamaged;
    private const float minTimeBetDamaged = 0.5f;

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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sword") == true)
        {
            LivingEntity player = other.transform.root.GetComponent<LivingEntity>();

            if (player != null && player.canAttack == true)
            {
                if (ApplyDamage(player) == true)
                {
                    lastTimeDamaged = Time.time;

                    var hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = (transform.position - hitPoint).normalized;
                    EffectManager.Instance.PlayHitEffect(hitPoint, hitNormal, other.transform, EffectManager.EffectType.Flesh);
                }
            }
        }
    }
}
