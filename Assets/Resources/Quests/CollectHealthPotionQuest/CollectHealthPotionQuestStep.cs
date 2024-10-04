using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectHealthPotionQuestStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;

    private string targetItemName;
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
    }
    private void Start()
    {
        questOwner.GetComponent<PlayerQuestEvent>().onCollect += CollectHealthPotion;

        targetItemName = targetItem.tag;
       
        currentHealthPotionCount = questOwner.GetComponent<Inventory>().FindItem(targetItemName);
        status = "HealthPotion Collected: " + currentHealthPotionCount;

        if (currentHealthPotionCount >= targetHealthPotionCount)
        {
            FinishQuestStep(QuestStepState.FINISHED);
        }
    }

    private void CollectHealthPotion()
    {
        currentHealthPotionCount = questOwner.GetComponent<Inventory>().FindItem(targetItemName);
        status = "HealthPotion Collected: " + currentHealthPotionCount;

        if (currentHealthPotionCount >= targetHealthPotionCount)
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
