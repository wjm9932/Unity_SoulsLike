using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [System.Serializable]
    public struct QuestInfo
    {
        public QuestInfoSO quest;
        public bool isStartPoint;
        public bool isFinishPoint;
    }

    [Header("Quest")]
    [SerializeField] private QuestInfo[] quests;

    [Header("Config")]

    private int questIndex;
    private QuestIcon questIcon;
    private QuestState currentQuestState;
    private bool isPlayerNearby = false;

    private void Awake()
    {
        questIndex = 0;
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
        if (isPlayerNearby == false)
        {
            return false;
        }
        if (currentQuestState == QuestState.REQUIREMENTS_NOT_MET && quests[questIndex].isStartPoint == true)
        {
            QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].quest.displayName, quests[questIndex].quest.requirementsNotMetDialogue);
        }
        else if (currentQuestState == QuestState.CAN_START && quests[questIndex].isStartPoint == true)
        {
            QuestManager.Instance.StartQuest(quests[questIndex].quest.id);
            QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].quest.displayName, quests[questIndex].quest.onStartDialogue);
        }
        else if (currentQuestState == QuestState.CAN_FINISH && quests[questIndex].isFinishPoint == true)
        {
            if (QuestManager.Instance.FinishQuest(quests[questIndex].quest.id) == true)
            {
                QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].quest.displayName, quests[questIndex].quest.onFinishDialogue);
            }
            else
            {
                QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].quest.displayName, quests[questIndex].quest.onFailFinishDialogue);
            }
        }
        else if (currentQuestState == QuestState.FINISHED)
        {
            QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].quest.displayName, quests[questIndex].quest.doneDialogue);
        }
        else if (currentQuestState == QuestState.IN_PROGRESS && quests[questIndex].isFinishPoint == true)
        {
            QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].quest.displayName, quests[questIndex].quest.id);
        }
        else
        {
            QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].quest.displayName, "Hey");
        }
        return true;
    }

    private void ChangeQuestState(Quest quest)
    {
        if (quests[questIndex].quest.id == quest.info.id)
        {
            currentQuestState = quest.state;

            if (currentQuestState == QuestState.FINISHED)
            {
                if (MoveToNextQuest() == true)
                {
                    currentQuestState = quests[questIndex].quest.initialState;
                }
            }

            questIcon.SetState(currentQuestState, quests[questIndex].isStartPoint, quests[questIndex].isFinishPoint);
        }
    }

    private bool MoveToNextQuest()
    {
        if (questIndex + 1 < quests.Length)
        {
            questIndex++;
            return true;
        }

        QuestManager.Instance.onChangeQuestState -= ChangeQuestState;
        return false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            isPlayerNearby = true;
            other.GetComponent<InteractionIndicator>().Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            isPlayerNearby = false;
            other.GetComponent<InteractionIndicator>().Hide();
        }
    }
}
