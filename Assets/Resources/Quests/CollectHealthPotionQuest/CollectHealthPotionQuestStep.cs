using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectHealthPotionQuestStep : QuestStep
{
    private string targetItemName = "HealthPotion";
    
    private int currentHealthPotionCount = 0;

    [SerializeField]
    private int targetHealthPotionCount = 5;

    private void OnDisable()
    {
        if(questOwner != null)
        {
            questOwner.GetComponent<PlayerQuestEvent>().onCollect -= CollectHealthPotion;
        }
    }

    private void Start()
    {
        questOwner.GetComponent<PlayerQuestEvent>().onCollect += CollectHealthPotion;

        currentHealthPotionCount = questOwner.GetComponent<Inventory>().FindItem(targetItemName);
        if (currentHealthPotionCount >= targetHealthPotionCount)
        {
            FinishQuestStep();
        }
    }

    private void CollectHealthPotion()
    {
        currentHealthPotionCount = questOwner.GetComponent<Inventory>().FindItem(targetItemName);
        if(currentHealthPotionCount >= targetHealthPotionCount)
        {
            FinishQuestStep();
        }
        else
        {
            UpdateQuestStep();
        }
    }
}
