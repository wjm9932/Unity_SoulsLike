using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBoneStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;

    private string targetItemName;
    private int currentBoneCount = 0;

    [SerializeField]
    private int targetBoneCount = 5;

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerQuestEvent>().onCollect -= CollectBone;
        }
    }
    private void Awake()
    {
    }
    private void Start()
    {
        targetItemName = targetItem.tag;
        questOwner.GetComponent<PlayerQuestEvent>().onCollect += CollectBone;

        currentBoneCount = questOwner.GetComponent<Inventory>().FindItem(targetItemName);

        status = "Bone Collected: " + currentBoneCount;
        if (currentBoneCount >= targetBoneCount)
        {
            FinishQuestStep(QuestStepState.FINISHED);
        }
    }

    private void CollectBone()
    {
        currentBoneCount = questOwner.GetComponent<Inventory>().FindItem(targetItemName);

        status = "Bone Collected: " + currentBoneCount;

        if (currentBoneCount >= targetBoneCount)
        {
            if (isFinished == false)
            {
                FinishQuestStep(QuestStepState.FINISHED);
            }
            else
            {
                UpdateQuestStepState(QuestStepState.FINISHED);
            }
        }
        else
        {
            UpdateQuestStepState(QuestStepState.IN_PROGRESS);
        }
    }
}
