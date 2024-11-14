using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTargetQuestStep : QuestStep
{
    [SerializeField] private int targetCount;
    private int currnetCount = 0;
    private void Awake()
    {
        status = "Objective: Change lock on target: " + currnetCount.ToString() + "/" + targetCount.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        status = "Change lock on target: " + currnetCount.ToString() + "/" + targetCount.ToString();
        questOwner.playerEvents.onChangeTarget += ChangeTarget;
    }

    private void ChangeTarget()
    {
        ++currnetCount;
        status = "Change lock on target: " + currnetCount.ToString() + "/" + targetCount.ToString();

        if (currnetCount >= targetCount)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
            questOwner.playerEvents.onChangeTarget -= ChangeTarget;
        }
        else
        {
            UpdateQuestStepState(QuestStepState.IN_PROGRESS);
        }
    }

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.playerEvents.onChangeTarget -= ChangeTarget;
        }
    }
}
