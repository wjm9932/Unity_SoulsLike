using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuestInfoSO;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state { get; private set; }
    public List<GameObject> questSteps { get; private set; } = new List<GameObject>();
    public QuestStepData[][] questStepData;

    private Character questOwner;
    private int currentQuestStepIndex;
    
    public Quest(QuestInfoSO info, Character owner)
    {
        this.info = info;
        this.state = info.initialState;
        this.currentQuestStepIndex = 0;
        this.questOwner = owner;

        questStepData = new QuestStepData[info.questStepPrefabs.Length][];

        for (int i = 0; i < info.questStepPrefabs.Length; i++)
        {
            questStepData[i] = new QuestStepData[info.questStepPrefabs[i].stepPrefabs.Length];

            for (int j = 0; j < info.questStepPrefabs[i].stepPrefabs.Length; j++)
            {
                questStepData[i][j] = new QuestStepData();
            }
        }
    }

    public void LoadQuestData(QuestState state, int currentQuestStepIndex, QuestStepData[][] questStepData)
    {
        if(this.questStepData.Length != questStepData.Length)
        {
            Debug.LogError("Load sync failed");
            return;
        }

        this.state = state;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepData = questStepData;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool IsNextQuestStepExists()
    {
        if(currentQuestStepIndex + 1 < info.questStepPrefabs.Length)
        {
            ++currentQuestStepIndex;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        QuestStepPrefabs questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            for(int i = 0; i < questStepPrefab.stepPrefabs.Length; i++)
            {
                GameObject questStep = Object.Instantiate<GameObject>(questStepPrefab.stepPrefabs[i], parentTransform);
                questStep.GetComponent<QuestStep>().Initialize(questOwner, info.id, questStepData[currentQuestStepIndex][i]);
                questSteps.Add(questStep);
            }
        }
    }

    public void InstantiateLoadedQuestStep(Transform parentTransform)
    {
        for(int i = 0; i <= currentQuestStepIndex; i++)
        {
            QuestStepPrefabs questStepPrefab = info.questStepPrefabs[i];

            for (int j = 0; j < questStepPrefab.stepPrefabs.Length; j++)
            {
                GameObject questStep = Object.Instantiate<GameObject>(questStepPrefab.stepPrefabs[j], parentTransform);
                questStep.GetComponent<QuestStep>().Initialize(questOwner, info.id, questStepData[i][j]);
                questSteps.Add(questStep);
            }
        }
    }

    public void SetQuestState(QuestState state)
    {
        this.state = state;
    }
    private QuestStepPrefabs GetCurrentQuestStepPrefab()
    {
        QuestStepPrefabs questStepPrefab = null;
        questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];

        if(questStepPrefab == null)
        {
            Debug.LogError("questStepPrefab is null");
        }
       
        return questStepPrefab;
    }

    public bool GetReward()
    {
        for (int i = 0; i < info.rewards.Length; i++)
        {
            if (questOwner.inventory.HasEnoughSpace(info.rewards[i].itemPrefab) == false)
            {
                return false;
            }
        }
        
        for(int i = 0; i < info.targetItem.Length; i++)
        {
            if (questOwner.inventory.GetTargetItemCountFromInventory(info.targetItem[i].itemPrefab) < info.targetItem[i].count)
            {
                return false;
            }
        }

        for (int i = 0; i < info.rewards.Length; i++)
        {
            var rewardItem = info.rewards[i];
            questOwner.inventory.AddItem(rewardItem.itemPrefab, rewardItem.count);
            TextManager.Instance.PlayNotificationText("You've got reward: " + rewardItem.itemPrefab.data.itemName + " x" + rewardItem.count);
        }

        for (int i = 0; i < info.targetItem.Length; i++)
        {
            var targetItem = info.targetItem[i];
            questOwner.inventory.RemoveTargetItemFromInventory(targetItem.itemPrefab, targetItem.count);
        }

        return true;
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
            for(int i = 0; i <= currentQuestStepIndex; i++)
            {
                for(int j = 0; j < questStepData[i].Length; j++)
                {
                    if (questStepData[i][j].questStepState == QuestStepState.FINISHED)
                    {
                        fullStatus += "<s>" + questStepData[i][j].status + "</s>\n";
                    }
                    else
                    {
                        fullStatus +=  questStepData[i][j].status + "\n";
                    }
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
        for (int i = 0; i < questSteps.Count; i++)
        {
            Object.Destroy(questSteps[i].gameObject);
        }
        questSteps.Clear();
    }

    public bool CanMoveNextQuestStep()
    {
        for(int i = 0; i <= currentQuestStepIndex; i++)
        {
            for(int j = 0; j < questStepData[i].Length; j++)
            {
                if (questStepData[i][j].questStepState != QuestStepState.FINISHED)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public QuestData GetQuestSaveData()
    {
        return new QuestData(info.id, state, currentQuestStepIndex, Convert1DArrayFrom2DArray(questStepData));
    }

    private QuestStepData[] Convert1DArrayFrom2DArray(QuestStepData[][] questStepData)
    {
        int totalLength = 0;
        for (int i = 0; i < info.questStepPrefabs.Length; i++)
        {
            totalLength += info.questStepPrefabs[i].stepPrefabs.Length;
        }

        QuestStepData[] questStepData1D = new QuestStepData[totalLength];

        int index = 0;
        for (int i = 0; i < info.questStepPrefabs.Length; i++)
        {
            for (int j = 0; j < info.questStepPrefabs[i].stepPrefabs.Length; j++)
            {
                questStepData1D[index++] = questStepData[i][j];
            }
        }

        return questStepData1D;
    }

    public static QuestStepData[][] Convert2DArrayFrom1DArray(QuestStepData[] questStepData, QuestInfoSO info)
    {
        QuestStepData[][] questStepData2D = new QuestStepData[info.questStepPrefabs.Length][];

        int index = 0;
        for (int i = 0; i < info.questStepPrefabs.Length; i++)
        {
            questStepData2D[i] = new QuestStepData[info.questStepPrefabs[i].stepPrefabs.Length];
            for (int j = 0; j < info.questStepPrefabs[i].stepPrefabs.Length; j++)
            {
                questStepData2D[i][j] = questStepData[index++];
            }
        }

        return questStepData2D;
    }

    public QuestStepData[][] Convert2DArrayFrom1DArray(QuestStepData[] questStepData)
    {
        QuestStepData[][] questStepData2D = new QuestStepData[info.questStepPrefabs.Length][];

        int index = 0;
        for (int i = 0; i < info.questStepPrefabs.Length; i++)
        {
            questStepData2D[i] = new QuestStepData[info.questStepPrefabs[i].stepPrefabs.Length];
            for (int j = 0; j < info.questStepPrefabs[i].stepPrefabs.Length; j++)
            {
                questStepData2D[i][j] = questStepData[index++];
            }
        }

        return questStepData2D;
    }
}
