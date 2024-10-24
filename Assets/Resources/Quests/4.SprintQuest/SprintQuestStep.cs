using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintQuestStep : QuestStep
{
    [SerializeField] private float targetSprintTime;
    private float currentSprintTime;
    private void Awake()
    {
        currentSprintTime = 0f;
        status = "Objective: Sprint for " + targetSprintTime.ToString() + " seconds!";
    }
    // Start is called before the first frame update
    void Start()
    {
        status = "Sprint time: " + currentSprintTime.ToString();
        questOwner.playerEvents.onSprint += Sprint;
    }

    private void Sprint(float deltaTime)
    {
        currentSprintTime += deltaTime;
        status = "Sprint time: " + currentSprintTime.ToString("F2");

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
}
