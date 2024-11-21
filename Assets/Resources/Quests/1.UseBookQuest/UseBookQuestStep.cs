using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseBookQuestStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;

    private void Awake()
    {
        //questStepData.status = "Objective: Use a Book\n";
    }
    // Start is called before the first frame update
    void Start()
    {
        questOwner.GetComponent<PlayerEvent>().onUse += UseBook;
        questStepData.status = "Use a Book";
    }


    private void UseBook(string itemName)
    {
        if (itemName == targetItem.GetComponent<UX_Item>().data.itemName)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
            questOwner.GetComponent<PlayerEvent>().onUse -= UseBook;
        }
    }

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerEvent>().onUse -= UseBook;
        }
    }
}