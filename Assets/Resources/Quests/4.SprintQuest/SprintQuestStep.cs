using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintQuestStep : QuestStep, ICountBasedQuest
{
    [SerializeField] private float targetSprintTime;
    private float currentSprintTime;

    void Start()
    {
        UpdateStatus();
        questOwner.playerEvents.onSprint += Sprint;
    }

    private void Sprint(float deltaTime)
    {
        currentSprintTime += deltaTime;
        UpdateStatus();

        if (currentSprintTime > targetSprintTime)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
            questOwner.playerEvents.onSprint -= Sprint;
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
            questOwner.playerEvents.onSprint -= Sprint;
        }
    }
    private void UpdateStatus()
    {
        questStepData.count = currentSprintTime.ToString();
        questStepData.status = "Sprint time: " + currentSprintTime.ToString("F2") + "s";
    }
    public void SetQuestStepCount(string count)
    {
        currentSprintTime = float.Parse(count);
    }
}
