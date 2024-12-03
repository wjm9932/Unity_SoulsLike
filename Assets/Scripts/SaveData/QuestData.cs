using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public string questId;
    public QuestState state;
    public int currentQuestStepIndex;
    public QuestStepData[] questStepData;

    public QuestData(string questId, QuestState state, int currentQuestStepIndex, QuestStepData[] questStepData)
    {
        this.questId = questId;
        this.state = state;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepData = questStepData;
    }
}
