using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO quest;

    [Header("Config")]
    [SerializeField] private bool isStartPoint = true;
    [SerializeField] private bool isFinishPoint = true;


    private QuestIcon questIcon;
    private QuestState currentQuestState;
    private bool isPlayerNearby = false;
    private void Awake()
    {
        questIcon = GetComponentInChildren<QuestIcon>();
    }
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

        if (currentQuestState == QuestState.CAN_START && isStartPoint == true)
        {
            QuestManager.Instance.StartQuest(quest.id);
        }
        else if (currentQuestState == QuestState.CAN_FINISH && isFinishPoint == true)
        {
            QuestManager.Instance.FinishQuest(quest.id);
        }
        else
        {
            Debug.Log("Can't start or finish quest");
        }
    }

    private void ChangeQuestState(Quest quest)
    {
        if (this.quest.id == quest.info.id)
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, isStartPoint, isFinishPoint);

            Debug.Log("Quest with id: " + this.quest.id + " is updated to state: " + currentQuestState);
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
