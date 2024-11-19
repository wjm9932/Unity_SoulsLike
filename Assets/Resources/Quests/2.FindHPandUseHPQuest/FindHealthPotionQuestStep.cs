using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindHealthPotionQuestStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerEvent>().onCollect -= FindHealthPotion;
        }
    }
    void Start()
    {
        questOwner.GetComponent<PlayerEvent>().onCollect += FindHealthPotion;
        questStepData.status = "Find a Health Potion";

        if (questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.UX_Item>()) >= 1)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }
    private void FindHealthPotion(string itemName)
    {
        if(itemName == targetItem.GetComponent<UX.UX_Item>().itemName)
        {
            if (questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.UX_Item>()) >= 1)
            {
                UpdateQuestStepState(QuestStepState.FINISHED);
                questOwner.GetComponent<PlayerEvent>().onCollect -= FindHealthPotion;
            }
        }
    }
}
