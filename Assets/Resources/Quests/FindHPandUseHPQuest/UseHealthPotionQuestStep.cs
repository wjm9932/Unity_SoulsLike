using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHealthPotionQuestStep : QuestStep
{
    [SerializeField]
    private GameObject targetItem;

    private void Awake()
    {
        status = "Objective: Use Health Potion\n";
    }
    // Start is called before the first frame update
    void Start()
    {
        questOwner.GetComponent<PlayerQuestEvent>().onUse += UseHealthPotion;
        status = "Use Health Potion\n";
    }


    private void UseHealthPotion(string itemName)
    {
        if(itemName == targetItem.GetComponent<UX.UX_Item>().itemName)
        {
            UpdateQuestStepState(QuestStepState.FINISHED);
        }
    }

    private void OnDisable()
    {
        if (questOwner != null)
        {
            questOwner.GetComponent<PlayerQuestEvent>().onUse -= UseHealthPotion;
        }
    }
}
