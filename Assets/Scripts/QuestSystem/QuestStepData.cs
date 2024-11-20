using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class QuestStepData
{
    public QuestStepState questStepState;
    public string status;
    public string count;

    public QuestStepData()
    {
        this.questStepState = QuestStepState.IN_PROGRESS;
        this.status = "";
        this.count = "";
    }
}
