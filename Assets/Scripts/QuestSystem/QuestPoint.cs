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


    private bool InteractWithQuest()
    {
        if(isPlayerNearby == false)
        {
            return false;
        }
        if(currentQuestState == QuestState.REQUIREMENTS_NOT_MET && isStartPoint == true)
        {
            QuestManager.Instance.UpdateQuestDialogue(quest.displayName, quest.requirementsNotMetDialogue);
        }
        else if (currentQuestState == QuestState.CAN_START && isStartPoint == true)
        {
            QuestManager.Instance.StartQuest(quest.id);
            QuestManager.Instance.UpdateQuestDialogue(quest.displayName, quest.onStartDialogue);
        }
        else if (currentQuestState == QuestState.CAN_FINISH && isFinishPoint == true)
        {
            QuestManager.Instance.FinishQuest(quest.id);
            QuestManager.Instance.UpdateQuestDialogue(quest.displayName, quest.onFinishDialogue);
        }
        else if (currentQuestState == QuestState.FINISHED)
        {
            QuestManager.Instance.UpdateQuestDialogue(quest.displayName, quest.doneDialogue);
        }
        else
        {
            QuestManager.Instance.UpdateQuestDialogue(quest.displayName, quest.id);
        }
        return true;
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
        if (other.CompareTag("Player") == true)
        {
            isPlayerNearby = false;
        }
    }
}
