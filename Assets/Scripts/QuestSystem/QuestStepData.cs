using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestStepData
{
    public string status;
    public QuestStepState state;

    public QuestStepData()
    {
        this.status = "";
        state = QuestStepState.IN_PROGRESS;
    }
}
