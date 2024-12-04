using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBuff : Buff
{
    public override void Initialize(float value)
    {
        elapsedTime = duration;
        owner.playerBuffManager.buffDamage = value;
    }

    private void OnDisable()
    {
        owner.playerBuffManager.buffDamage = 0f;
    }
}
