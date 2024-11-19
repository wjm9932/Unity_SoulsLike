using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStepData
{
    public QuestStepState questStepState;
    public string status;

    public QuestStepData()
    {
        questStepState = QuestStepState.IN_PROGRESS;
        this.status = "";
    }
}
