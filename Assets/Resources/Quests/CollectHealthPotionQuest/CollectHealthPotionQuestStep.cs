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
            questOwner.GetComponent<PlayerQuestEvent>().onCollect -= CollectHealthPotion;
        }
    }
    private void Awake()
    {
        status = "Target item: Health Potion\n";
        status += "Target count: " + targetHealthPotionCount + "\n";
    }
    private void Start()
    {
        questOwner.GetComponent<PlayerQuestEvent>().onCollect += CollectHealthPotion;
       
        currentHealthPotionCount = questOwner.GetComponent<Inventory>().FindItemFromInventory(targetItem);
        status = "HealthPotion Collected: " + currentHealthPotionCount + "/" + targetHealthPotionCount;
        
        if (currentHealthPotionCount >= targetHealthPotionCount)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }

    private void CollectHealthPotion()
    {
        currentHealthPotionCount = questOwner.GetComponent<Inventory>().FindItemFromInventory(targetItem);
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
