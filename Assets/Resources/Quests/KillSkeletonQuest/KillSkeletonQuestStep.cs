using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSkeletonQuestStep : QuestStep
{
    private int currentKillCount = 0;
    [SerializeField] private int targetKillCount = 5;
    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerQuestEvent>().OnKill -= KillSkeleton;
        }
    }
    private void Awake()
    {
        status = "Objective: Kill 5 Skeletons\n";
    }

    // Start is called before the first frame update
    void Start()
    {
        questOwner.GetComponent<PlayerQuestEvent>().OnKill += KillSkeleton;
        status = "Kill Skeleton: " + currentKillCount + "/" + targetKillCount;
    }

    private void KillSkeleton()
    {
        ++currentKillCount;
        status = "Kill Skeleton: " + currentKillCount + "/" + targetKillCount;

        if (currentKillCount > targetKillCount)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
        else
        {
            UpdateQuestStepState(QuestStepState.IN_PROGRESS);
        }
    }

}
