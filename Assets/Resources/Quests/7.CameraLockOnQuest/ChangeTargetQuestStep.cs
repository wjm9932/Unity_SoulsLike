using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTargetQuestStep : QuestStep, ICountBasedQuest
{
    [SerializeField] private int targetCount;
    private int currnetCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        questOwner.playerEvents.onChangeTarget += ChangeTarget;
        UpdateStatus();
    }

    private void ChangeTarget()
    {
        ++currnetCount;
        UpdateStatus();

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
    private void UpdateStatus()
    {
        questStepData.count = currnetCount.ToString();
        questStepData.status = "Change lock on target: " + currnetCount.ToString() + "/" + targetCount.ToString();
    }
    public void SetQuestStepCount(string count)
    {
        currnetCount = System.Int32.Parse(count);
    }
}
