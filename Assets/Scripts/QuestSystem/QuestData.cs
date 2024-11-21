using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public QuestState state;
    public int currentQuestStepIndex;
    public QuestStepData[] questStepData;

    public QuestData(QuestState state, int currentQuestStepIndex, QuestStepData[] questStepData)
    {
        this.state = state;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepData = questStepData;
    }
}
