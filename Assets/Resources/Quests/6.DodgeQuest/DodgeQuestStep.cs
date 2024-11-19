using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeQuestStep : QuestStep
{
    // Start is called before the first frame update
    void Start()
    {
        questStepData.status = "Press Space to Dodge";
        questOwner.playerEvents.onDodge += Dodge;
    }

    private void Dodge()
    {
        UpdateQuestStepState(QuestStepState.FINISHED);
        questOwner.playerEvents.onDodge -= Dodge;
    }

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.playerEvents.onDodge -= Dodge;
        }
    }
}
