using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockOnQuestStep : QuestStep
{
    private void Awake()
    {
        status = "Objective: Lock on to enemy";
    }
    // Start is called before the first frame update
    void Start()
    {
        status = "Press F to Lock On";
        questOwner.playerEvents.onCameraLockOn += CameraLockOn;
    }

    private void CameraLockOn()
    {
        UpdateQuestStepState(QuestStepState.FINISHED);
        questOwner.playerEvents.onCameraLockOn -= CameraLockOn;
    }

    private void OnDisable()
    {
        questOwner.playerEvents.onCameraLockOn -= CameraLockOn;
    }
}

