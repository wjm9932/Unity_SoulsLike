using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    protected bool isFinished;
    protected string questId;
    protected Character questOwner;
    public QuestStepState state { get; protected set; } = QuestStepState.IN_PROGRESS;

    protected void FinishQuestStep(QuestStepState state)
    {
        if (this.state != state)
        {
            this.state = state;
            QuestManager.Instance.AdvanceQuest(questId);
            isFinished = true;
        }
    }
    protected void UpdateQuestStepState(QuestStepState state)
    {
        if (this.state != state)
        {
            this.state = state;
            QuestManager.Instance.UpdateQuestState(questId);
        }
    }
    public void Initialize(Character owner, string id)
    {
        questOwner = owner;
        questId = id;
    }
}
