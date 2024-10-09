using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemy : Enemy
{
    public Character target { get; set; }

    protected override void Awake()
    {
        base.Awake();

        entityType = EntityType.ENEMY;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        health = 10f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetNavMeshArea(string areaName)
    {
        int areaMask = NavMesh.GetAreaFromName(areaName);
        navMesh.areaMask = 1 << areaMask;
    }

    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject, 3f);
        GetComponent<Collider>().enabled = false;
        animator.SetTrigger("Die");
    }
}
