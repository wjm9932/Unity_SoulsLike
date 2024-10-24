using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO[] quests;

    [Header("Config")]
    [SerializeField] private bool isStartPoint = true;
    [SerializeField] private bool isFinishPoint = true;

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
        if(isPlayerNearby == false)
        {
            return false;
        }
        if(currentQuestState == QuestState.REQUIREMENTS_NOT_MET && isStartPoint == true)
        {
            QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].displayName, quests[questIndex].requirementsNotMetDialogue);
        }
        else if (currentQuestState == QuestState.CAN_START && isStartPoint == true)
        {
            QuestManager.Instance.StartQuest(quests[questIndex].id);
            QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].displayName, quests[questIndex].onStartDialogue);
        }
        else if (currentQuestState == QuestState.CAN_FINISH && isFinishPoint == true)
        {
            if(QuestManager.Instance.FinishQuest(quests[questIndex].id, ref questIndex) == true)
            {
                QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].displayName, quests[questIndex].onFinishDialogue);
            }
            else
            {
                QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].displayName, quests[questIndex].onFailFinishDialogue);
            }
        }
        else if (currentQuestState == QuestState.FINISHED)
        {
            QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].displayName, quests[questIndex].doneDialogue);
        }
        else
        {
            QuestManager.Instance.UpdateQuestDialogue(quests[questIndex].displayName, quests[questIndex].id);
        }
        return true;
    }

    private void ChangeQuestState(Quest quest)
    {
        if (quests.Length <= questIndex)
        {
            questIndex = quests.Length - 1;
        }

        if (this.quests[questIndex].id == quest.info.id)
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, isStartPoint, isFinishPoint);

            //if (currentQuestState == QuestState.FINISHED)
            //{
            //    Destroy(this);
            //    return;
            //}
        }
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
