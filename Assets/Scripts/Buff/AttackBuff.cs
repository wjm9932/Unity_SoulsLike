using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBuff : Buff
{
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = duration;
        owner.buffDamage = 10f;
    }
    private void OnDisable()
    {
        owner.buffDamage = 0f;
    }
}
