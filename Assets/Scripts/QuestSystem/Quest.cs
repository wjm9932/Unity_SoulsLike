using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state { get; private set; }
    private int currentQuestStepIndex;
    private Character questOwner;
    private GameObject currentQuestStep;

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
            currentQuestStep = questStep.gameObject;
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

    public bool GetReward(string id, int count)
    {
        //questOwner.inventory.AddItem()
        //questOwner.inventory.RemoveItem()
        return true;
    }

    public void DestroyCurrentQuestStep()
    {
        if (currentQuestStep != null)
        {
            Object.Destroy(currentQuestStep);
        }
    }
}
