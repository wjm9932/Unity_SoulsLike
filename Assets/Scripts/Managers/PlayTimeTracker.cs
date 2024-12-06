using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTimeTracker: MonoBehaviour
{
    private static float playTime;

    private void Awake()
    {
        playTime = 0f;
    }
    public static float GetTotalPlayTime()
    {
        return playTime;
    }

    public static void LoadData(SlotData data)
    {
        playTime = data.totalPlayTime;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
    }
}
