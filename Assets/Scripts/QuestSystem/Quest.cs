using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static QuestInfoSO;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state { get; private set; }
    public List<GameObject> questSteps { get; private set; } = new List<GameObject>();
    public QuestStepData[] questStepData;

    private Character questOwner;
    private int currentQuestStepIndex;

    private int totalDataCount = 0;
    private int currentDataCount = 0;
    public Quest(QuestInfoSO info, Character owner)
    {
        this.info = info;
        this.state = info.initialState;
        this.currentQuestStepIndex = 0;
        this.questOwner = owner;

        for (int i = 0; i < info.questStepPrefabs.Length; i++)
        {
            totalDataCount += info.questStepPrefabs[i].stepPrefabs.Length;
        }

        questStepData = new QuestStepData[totalDataCount];

        for (int i = 0; i < totalDataCount; i++)
        {
            questStepData[i] = new QuestStepData();
        }
    }
    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool IsNextQuestStepExists()
    {
        if (currentQuestStepIndex + 1 < info.questStepPrefabs.Length)
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
            for (int i = 0; i < questStepPrefab.stepPrefabs.Length; i++)
            {
                GameObject questStep = Object.Instantiate<GameObject>(questStepPrefab.stepPrefabs[i], parentTransform);
                questStep.GetComponent<QuestStep>().Initialize(questOwner, info.id, questStepData[currentDataCount++]);
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

        if (questStepPrefab == null)
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

        for (int i = 0; i < info.targetItem.Length; i++)
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
            TextManager.Instance.PlayNotificationText("You've got reward: " + rewardItem.itemPrefab.itemName + " x" + rewardItem.count);
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
            //for (int i = 0; i <= currentQuestStepIndex; i++)
            //{
            //    for (int j = 0; j < questStepData[i].Length; j++)
            //    {
            //        if (questStepData[i][j].questStepState == QuestStepState.FINISHED)
            //        {
            //            fullStatus += "<s>" + questStepData[i][j].status + "</s>\n";
            //        }
            //        else
            //        {
            //            fullStatus += questStepData[i][j].status + "\n";
            //        }
            //    }
            //}

            for (int i = 0; i < currentDataCount; i++)
            {
                if (questStepData[i].questStepState == QuestStepState.FINISHED)
                {
                    fullStatus += "<s>" + questStepData[i].status + "</s>\n";
                }
                else
                {
                    fullStatus += questStepData[i].status + "\n";
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
        //for (int i = 0; i <= currentQuestStepIndex; i++)
        //{
        //    for (int j = 0; j < questStepData[i].Length; j++)
        //    {
        //        if (questStepData[i][j].questStepState != QuestStepState.FINISHED)
        //        {
        //            return false;
        //        }
        //    }
        //}

        for (int i = 0; i < currentDataCount; i++)
        {
            if (questStepData[i].questStepState != QuestStepState.FINISHED)
            {
                return false;
            }
        }

        return true;
    }

    public QuestData GetQuestSaveData()
    {
        return new QuestData(state, currentQuestStepIndex, questStepData);
    }
}
