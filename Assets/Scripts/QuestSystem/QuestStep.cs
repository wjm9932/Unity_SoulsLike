using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished;
    private string questId;
    protected Character questOwner;
    protected void FinishQuestStep()
    {
        if(isFinished == false)
        {
            isFinished = true;
            QuestManager.Instance.AdvanceQuest(questId);
            Destroy(gameObject);
        }
    }
    public void Initialize(Character owner, string id)
    {
        questOwner = owner;
        questId = id;
    }
}
