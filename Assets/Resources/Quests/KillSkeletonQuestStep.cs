using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSkeletonQuestStep : QuestStep, ICountBasedQuest
{
    private int currentKillCount = 0;
    [SerializeField] private int targetKillCount;
    [SerializeField] private EntityType targetType;
    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerEvent>().OnKill -= KillSkeleton;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        questOwner.GetComponent<PlayerEvent>().OnKill += KillSkeleton;
        UpdateStatus();
    }

    private void KillSkeleton(EntityType type)
    {
        if(type == targetType)
        {
            ++currentKillCount;
            UpdateStatus();

            if (currentKillCount >= targetKillCount)
            {
                UpdateQuestStepState(QuestStepState.FINISHED);
            }
            else
            {
                UpdateQuestStepState(QuestStepState.IN_PROGRESS);
            }
        }
    }

    private void UpdateStatus()
    {
        questStepData.count = currentKillCount.ToString();
        questStepData.status = "Kill " + targetType.ToString() + " Skeleton: " + currentKillCount + "/" + targetKillCount;
    }

    public void SetQuestStepCount(string count)
    {
        currentKillCount = System.Int32.Parse(count);
    }
}
