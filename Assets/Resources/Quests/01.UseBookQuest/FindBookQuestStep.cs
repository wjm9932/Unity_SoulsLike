using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBookQuestStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerEvent>().onCollect -= FindBook;
        }
    }
    private void Awake()
    {
        //questStepData.status = "Objective: Find a Book\n";
    }

    void Start()
    {
        questOwner.GetComponent<PlayerEvent>().onCollect += FindBook;
        questStepData.status = "Find a Book";

        if (questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX_Item>()) >= 1)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }
    private void FindBook(string itemName)
    {
        if (itemName == targetItem.GetComponent<UX_Item>().data.itemName)
        {
            if (questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX_Item>()) >= 1)
            {
                UpdateQuestStepState(QuestStepState.FINISHED);
            }
        }
    }
}
