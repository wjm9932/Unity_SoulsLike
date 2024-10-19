using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBuff : Buff
{
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = duration;
        owner.buffArmorPercent = 0.3f;
    }
    private void OnDisable()
    {
        owner.buffArmorPercent = 0f;
    }
}
