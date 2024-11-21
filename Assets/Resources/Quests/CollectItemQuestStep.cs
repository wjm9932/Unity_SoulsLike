using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    private void Start()
    {
        questOwner.GetComponent<PlayerEvent>().onCollect += CollectItem;

        currentItemCount = questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX_Item>());
        questStepData.status = item.data.itemName + " Collected: " + currentItemCount + "/" + targetItemCount;

        if (currentItemCount >= targetItemCount)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }

    private void CollectItem(string itemName)
    {
        if (itemName == targetItem.GetComponent<UX_Item>().data.itemName)
        {
            currentItemCount = questOwner.inventory.GetTargetItemCountFromInventory(targetItem.GetComponent<UX_Item>());

            questStepData.status = item.data.itemName + " Collected: " + currentItemCount + "/" + targetItemCount;

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
