using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBoneStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;
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
        status = "Target item: Bone\n";
        status += "Target count: " + targetBoneCount + "\n";
    }
    private void Start()
    {
        questOwner.GetComponent<PlayerQuestEvent>().onCollect += CollectBone;

        currentBoneCount = questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.Item>());
        status = "Bone Collected: " + currentBoneCount + "/" + targetBoneCount;

        if (currentBoneCount >= targetBoneCount)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }

    private void CollectBone(string itemName)
    {
        if (itemName == targetItem.GetComponent<UX.Item>().itemName)
        {
            currentBoneCount = questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.Item>());

            status = "Bone Collected: " + currentBoneCount + "/" + targetBoneCount;

            if (currentBoneCount >= targetBoneCount)
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
