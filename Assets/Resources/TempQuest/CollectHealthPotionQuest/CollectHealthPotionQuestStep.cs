using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectHealthPotionQuestStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;
    private int currentHealthPotionCount = 0;

    [SerializeField]
    private int targetHealthPotionCount = 5;

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerEvent>().onCollect -= CollectHealthPotion;
        }
    }
    private void Awake()
    {
        status = "Target item: Health Potion\n";
        status += "Target count: " + targetHealthPotionCount + "\n";
    }
    private void Start()
    {
        questOwner.GetComponent<PlayerEvent>().onCollect += CollectHealthPotion;
       
        currentHealthPotionCount = questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.UX_Item>());
        status = "HealthPotion Collected: " + currentHealthPotionCount + "/" + targetHealthPotionCount;
        
        if (currentHealthPotionCount >= targetHealthPotionCount)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }

    private void CollectHealthPotion(string itemName)
    {
        if(itemName == targetItem.GetComponent<UX.UX_Item>().itemName)
        {
            currentHealthPotionCount = questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.UX_Item>());
            status = "HealthPotion Collected: " + currentHealthPotionCount + "/" + targetHealthPotionCount;

            if (currentHealthPotionCount >= targetHealthPotionCount)
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
