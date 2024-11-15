using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UX;

public class CollectItemQuestStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;
    private int currentItemCount = 0;
    private UX_Item item;

    [SerializeField]
    private int targetItemCount;

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerEvent>().onCollect -= CollectItem;
        }
    }
    private void Awake()
    {
        item = targetItem.GetComponent<UX_Item>();
        if(item == null)
        {
            Debug.LogError("Invalid target item");
        }

        status = "Objective: Get " + targetItemCount + item.itemName + "\n";
    }
    private void Start()
    {
        questOwner.GetComponent<PlayerEvent>().onCollect += CollectItem;

        currentItemCount = questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.UX_Item>());
        status = item.itemName + " Collected: " + currentItemCount + "/" + targetItemCount;

        if (currentItemCount >= targetItemCount)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }

    private void CollectItem(string itemName)
    {
        if (itemName == targetItem.GetComponent<UX.UX_Item>().itemName)
        {
            currentItemCount = questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX.UX_Item>());

            status = item.itemName + " Collected: " + currentItemCount + "/" + targetItemCount;

            if (currentItemCount >= targetItemCount)
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
