using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    protected string questId;
    protected Character questOwner;
    public string status { get; protected set; }
    public QuestStepState state { get; protected set; } = QuestStepState.IN_PROGRESS;

    protected void UpdateQuestStepState(QuestStepState state)
    {
        if (this.state != state)
        {
            this.state = state;
            QuestManager.Instance.AdvanceQuest(questId);
        }
        else
        {
            QuestManager.Instance.UpdateQuestProgress(questId);
        }
    }
    public void Initialize(Character owner, string id)
    {
        questOwner = owner;
        questId = id;
    }
}
