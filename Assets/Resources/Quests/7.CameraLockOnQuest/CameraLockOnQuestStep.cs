using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockOnQuestStep : QuestStep
{
    // Start is called before the first frame update
    void Start()
    {
        questStepData.status = "Press F to Lock On";
        questOwner.playerEvents.onCameraLockOn += CameraLockOn;
    }

    private void CameraLockOn()
    {
        UpdateQuestStepState(QuestStepState.FINISHED);
        questOwner.playerEvents.onCameraLockOn -= CameraLockOn;
    }

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.playerEvents.onCameraLockOn -= CameraLockOn;
        }
    }
}

