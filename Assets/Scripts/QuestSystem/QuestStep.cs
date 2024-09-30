using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished;
    protected Character questOwner;

    protected void FinishQuestStep()
    {
        if(isFinished == false)
        {
            isFinished = true;
            Destroy(gameObject);
        }
    }
    public void SetOwner(Character owner)
    {
        questOwner = owner;
    }
}
