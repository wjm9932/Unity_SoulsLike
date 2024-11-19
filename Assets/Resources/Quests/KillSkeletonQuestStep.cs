using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSkeletonQuestStep : QuestStep
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
        questStepData.status = "Kill " + targetType.ToString() + " Skeleton: " + currentKillCount + "/" + targetKillCount;
    }

    private void KillSkeleton(EntityType type)
    {
        if(type == targetType)
        {
            ++currentKillCount;
            questStepData.status = "Kill " + targetType.ToString() + " Skeleton: " + currentKillCount + "/" + targetKillCount;

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

}
