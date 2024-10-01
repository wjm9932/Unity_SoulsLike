using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField]
    private QuestInfoSO quest;

    private QuestState currentQuestState;
    private bool isPlayerNearby = false;

    private void OnEnable()
    {
        QuestManager.Instance.onChangeQuestState += ChangeQuestState;
        QuestManager.Instance.onInteractWithQuest += InteractWithQuest;
    }

    private void OnDisable()
    {
        QuestManager.Instance.onChangeQuestState -= ChangeQuestState;
        QuestManager.Instance.onInteractWithQuest -= InteractWithQuest;
    }

    private void InteractWithQuest()
    {
        if(isPlayerNearby == false)
        {
            return;
        }

        QuestManager.Instance.StartQuest(quest.id);
        QuestManager.Instance.AdvanceQuest(quest.id);
        QuestManager.Instance.FinishQuest(quest.id);

        //if(currentQuestState == QuestState.CAN_START)
        //{
        //    QuestManager.Instance.StartQuest(quest.id);
        //}
        //else if(currentQuestState == QuestState.CAN_FINISH)
        //{
        //    QuestManager.Instance.FinishQuest(quest.id);
        //}
        //else
        //{
        //    Debug.Log("Can't start or finish quest");
        //}
    }

    private void ChangeQuestState(Quest quest)
    {
        if (this.quest.id == quest.info.id)
        {
            currentQuestState = quest.state;
            Debug.Log("Quest with id: " + this.quest.id + "updated to state: " + currentQuestState);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == false)
        {
            isPlayerNearby = false;
        }
    }
}
