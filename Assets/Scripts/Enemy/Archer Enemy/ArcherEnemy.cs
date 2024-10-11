using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : Enemy
{
    public LayerMask whatIsTarget;
    public Transform eyeTransform;
    public GameObject arrow;
    public GameObject target { get; set; }
    public float viewDistance { get; private set; }
    public float fieldOfView { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        navMesh.updateRotation = false;
        entityType = EntityType.ARCHER;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
