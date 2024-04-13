using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public bool canAttack { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        canAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    public void SetCanAttack(bool canAttack)
    {
        this.canAttack = canAttack;
    }
}
