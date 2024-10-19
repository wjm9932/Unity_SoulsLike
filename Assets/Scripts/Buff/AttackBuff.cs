using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBuff : Buff
{
    public override void Initialize(float value)
    {
        elapsedTime = duration;
        owner.buffDamage = value;
    }

    private void OnDisable()
    {
        owner.buffDamage = 0f;
    }
}
