using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeQuestStep : QuestStep
{
    private void Awake()
    {
        status = "Objective: Dodge";
    }
    // Start is called before the first frame update
    void Start()
    {
        status = "Press Space to Dodge";
        questOwner.playerEvents.onDodge += Dodge;
    }

    private void Dodge()
    {
        UpdateQuestStepState(QuestStepState.FINISHED);
        questOwner.playerEvents.onDodge -= Dodge;
    }

    private void OnDisable()
    {
        questOwner.playerEvents.onDodge -= Dodge;
    }
}
