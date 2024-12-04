using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBuff : Buff
{
    public override void Initialize(float value)
    {
        elapsedTime = duration;
        owner.playerBuffManager.buffStaminaPercent = value;
    }
    private void OnDisable()
    {
        owner.playerBuffManager.buffStaminaPercent = 0f;
    }
}
