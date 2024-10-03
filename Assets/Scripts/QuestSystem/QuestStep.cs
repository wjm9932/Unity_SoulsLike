using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished;
    protected string questId;
    protected Character questOwner;
    protected QuestStepState state = QuestStepState.IN_PROGRESS;

    protected void FinishQuestStep(QuestStepState state)
    {
        if (this.state != state)
        {
            QuestManager.Instance.AdvanceQuest(questId);
            this.state = state;
        }
    }
    protected void UpdateQuestStep(QuestStepState state)
    {
        if (this.state != state)
        {
            QuestManager.Instance.ChangeQuestState(questId, QuestState.IN_PROGRESS);
            this.state = state;
        }
    }
    public void Initialize(Character owner, string id)
    {
        questOwner = owner;
        questId = id;
    }
}
