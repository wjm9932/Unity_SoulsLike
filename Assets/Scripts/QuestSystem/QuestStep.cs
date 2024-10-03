using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished;
    protected string questId;
    protected Character questOwner;

    protected void FinishQuestStep()
    {
        QuestManager.Instance.AdvanceQuest(questId);
    }
    protected void UpdateQuestStep()
    {
        QuestManager.Instance.ChangeQuestState(questId, QuestState.IN_PROGRESS);
    }
    public void Initialize(Character owner, string id)
    {
        questOwner = owner;
        questId = id;
    }
}
