using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintQuestStep : QuestStep
{
    [SerializeField] private float targetSprintTime;
    private float currentSprintTime;

    void Start()
    {
        questStepData.status = "Sprint time: " + currentSprintTime.ToString() + "s";
        questOwner.playerEvents.onSprint += Sprint;
    }

    private void Sprint(float deltaTime)
    {
        currentSprintTime += deltaTime;
        questStepData.status = "Sprint time: " + currentSprintTime.ToString("F2") + "s";

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
}
