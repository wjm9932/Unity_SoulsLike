using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    protected string questId;
    protected Character questOwner;

    protected QuestStepData questStepData;

    protected void UpdateQuestStepState(QuestStepState state)
    {
        if (this.questStepData.questStepState != state)
        {
            this.questStepData.questStepState = state;
            QuestManager.Instance.AdvanceQuest(questId);
        }
        else
        {
            QuestManager.Instance.UpdateQuestProgressWithID(questId);
        }
    }
    public void Initialize(Character owner, string id, QuestStepData questStepData)
    {
        questOwner = owner;
        questId = id;
        this.questStepData = questStepData;

        if(this.gameObject.GetComponent<ICountBasedQuest>() != null && this.questStepData.count != "")
        {
            this.gameObject.GetComponent<ICountBasedQuest>().SetQuestStepCount(this.questStepData.count);
        }
    }
}
