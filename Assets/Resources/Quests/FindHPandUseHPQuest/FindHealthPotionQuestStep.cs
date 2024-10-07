using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindHealthPotionQuestStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;

    private void OnDisable()
    {
        questOwner.GetComponent<PlayerQuestEvent>().onCollect -= FindHealthPotion;
    }
    private void Awake()
    {
        status = "Objective: Find and Get Health Potion\n";
    }

    void Start()
    {
        questOwner.GetComponent<PlayerQuestEvent>().onCollect += FindHealthPotion;
        status = "Find and Get Health Potion\n";

        if (questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.Item>()) >= 1)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }
    private void FindHealthPotion(string itemName)
    {
        if(itemName == targetItem.GetComponent<UX.Item>().itemName)
        {
            if (questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.Item>()) >= 1)
            {
                UpdateQuestStepState(QuestStepState.FINISHED);
            }
        }
    }
}
