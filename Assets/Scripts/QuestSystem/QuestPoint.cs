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
    }

    private void OnDisable()
    {
        QuestManager.Instance.onChangeQuestState -= ChangeQuestState;
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
