using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBuff : Buff
{
    public override void Initialize(float value)
    {
        elapsedTime = duration;
        owner.buffArmorPercent = value;
    }
    private void OnDisable()
    {
        owner.buffArmorPercent = 0f;
    }
}
