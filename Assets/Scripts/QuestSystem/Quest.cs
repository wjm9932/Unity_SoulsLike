using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state { get; private set; }
    private int currentQuestStepIndex;
    private Character questOwner;
    public List<QuestStep> questSteps { get; private set; } = new List<QuestStep>();

    public Quest(QuestInfoSO info, Character owner)
    {
        this.info = info;
        this.state = info.initialState;
        this.currentQuestStepIndex = 0;
        this.questOwner = owner;
    }
    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }
    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.Initialize(questOwner, info.id);
            questSteps.Add(questStep);
        }
    }
    public void ChangeQuestState(QuestState state)
    {
        this.state = state;
    }
    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists() == true)
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                + "there's no current step: QuestId=" + info.id + ", stepIndex=" + currentQuestStepIndex);
        }
        return questStepPrefab;
    }
    
    public bool GetReward()
    {
        if(questOwner.inventory.AddItem(info.rewardItem.GetComponent<UX.Item>(), info.rewardItemCount) == true)
        {
            if(questOwner.inventory.RemoveItemFromInventory(info.targetItem, info.targetItemCount) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public string GetFullStatusText()
    {
        string fullStatus = "";

        if (state == QuestState.REQUIREMENTS_NOT_MET)
        {
            fullStatus = "Requirements are not yet met to start this quest.";
        }
        else if (state == QuestState.CAN_START)
        {
            fullStatus = "This quest can be started!";
        }
        else
        {
            for(int i = 0; i < questSteps.Count; i++)
            {
                if (questSteps[i].state == QuestStepState.FINISHED)
                {
                    fullStatus += "<s>" + questSteps[i].status + "</s>\n";
                }
                else
                {
                    fullStatus += questSteps[i].status + "</s>\n";
                }
            }

            if (state == QuestState.CAN_FINISH)
            {
                fullStatus += "The quest is ready to be turned in.";
            }
            else if (state == QuestState.FINISHED)
            {
                fullStatus += "The quest has been completed!";
            }
        }

        return fullStatus;
    }

    public void DestroyQuestSteps()
    {
        for(int i = 0; i < questSteps.Count; i++)
        {
            Object.Destroy(questSteps[i].gameObject);
        }
    }
}
