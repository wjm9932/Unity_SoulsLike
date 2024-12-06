using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTimeTracker
{
    private static float playTime = 0f;
    public static float GetTotalPlayTime()
    {
        return Time.time + playTime;
    }

    public static void LoadData(SlotData data)
    {
        playTime = data.totalPlayTime;
    }
}
