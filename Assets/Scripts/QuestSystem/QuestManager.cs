using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using static QuestPoint;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public event Action<Quest> onChangeQuestState;
    public event Action<Quest> onUpdateQuestProgress;
    public event Action<Quest> onFinishQuest;
    public event Action<string, string> onUpdateQuestDialogue;
    public event Func<bool> onInteractWithQuest;

    [Header("Quest Owner")]
    [SerializeField] private Character questOwner;

    private Dictionary<string, Quest> questMap;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        else
        {
            Instance = this;
        }

        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            NotifyQuestStateToQuestPoints(quest);
        }
    }

    public void NotifyQuestFinished(Quest quest)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(quest);
        }
    }
    public void UpdateQuestDialogue(string questName, string text)
    {
        Quest quest = GetQuestById(text);
        if (quest != null)
        {
            if (onUpdateQuestDialogue != null)
            {
                onUpdateQuestDialogue(questName, quest.GetFullStatusText());
            }
        }
        else
        {
            onUpdateQuestDialogue(questName, text);
        }
    }

    public void UpdateQuestProgress(string id)
    {
        Quest quest = GetQuestById(id);

        if (onUpdateQuestProgress != null)
        {
            onUpdateQuestProgress(quest);
        }
    }

    private void UpdateQuestProgress(Quest quest)
    {
        if (onUpdateQuestProgress != null)
        {
            onUpdateQuestProgress(quest);
        }
    }

    public bool InteractWithQuest()
    {
        if (onInteractWithQuest != null)
        {
            foreach (Func<bool> del in onInteractWithQuest.GetInvocationList())
            {
                if (del.Invoke() == true)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest, QuestState.IN_PROGRESS);
    }

    public void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        UpdateQuestState(quest);
    }

    private void UpdateQuestState(Quest quest)
    {
        if (quest.CanMoveNextQuestStep() == false)
        {
            ChangeQuestState(quest, QuestState.IN_PROGRESS);
            return;
        }

        if (quest.IsNextQuestStepExists() == true)
        {
            quest.InstantiateCurrentQuestStep(this.transform);
            UpdateQuestProgress(quest);
        }
        else
        {
            ChangeQuestState(quest, QuestState.CAN_FINISH);
            TextManager.Instance.PlayNotificationText("Quest : " + "\"" + quest.info.displayName + "\"" + " can be completed!", Color.green);
            SoundManager.Instance.Play2DSoundEffect(SoundManager.UISoundEffectType.QUEST_COMPLETED, 0.25f);
        }
    }

    public bool FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest.GetReward() == false)
        {
            return false;
        }

        quest.DestroyQuestSteps();
        ChangeQuestState(quest, QuestState.FINISHED);
        NotifyQuestFinished(quest);
        FindCanStartQuest();
        return true;
    }

    private void FindCanStartQuest()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (IsRequirementMet(quest) == true && quest.state == QuestState.REQUIREMENTS_NOT_MET)
            {
                ChangeQuestState(quest, QuestState.CAN_START);
            }
        }
    }

    private bool IsRequirementMet(Quest quest)
    {
        if (quest.info.questPrerequisites.Length == 0)
        {
            return false;
        }

        foreach (QuestInfoSO prerequisites in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisites.id).state != QuestState.FINISHED)
            {
                return false;
            }
        }
        return true;
    }

    private void NotifyQuestStateToQuestPoints(Quest quest)
    {
        if (onChangeQuestState != null)
        {
            onChangeQuestState(quest);
        }
    }
    private void ChangeQuestState(Quest quest, QuestState state)
    {
        quest.SetQuestState(state);
        UpdateQuestProgress(quest);
        NotifyQuestStateToQuestPoints(quest);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id) == true)
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            else
            {
                idToQuestMap.Add(questInfo.id, new Quest(questInfo, questOwner));
            }
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        if (questMap.ContainsKey(id) == true)
        {
            return questMap[id];
        }
        else
        {
            return null;
        }

    }
    private void OnApplicationQuit()
    {
      
    }
    public void SaveQuest()
    {
        foreach (Quest quest in questMap.Values)
        {
            try
            {
                QuestData questData = quest.GetQuestSaveData();
                string serializedData = JsonUtility.ToJson(questData);
                PlayerPrefs.SetString(quest.info.id, serializedData);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to save quest with text " + quest.info.id + ": " + e);
            }
        }
    }

    public QuestData[] GetData()
    {
        QuestData[] data = new QuestData[questMap.Count];
        int index = 0;
        
        foreach(Quest quest in questMap.Values)
        {
            QuestData questData = quest.GetQuestSaveData();
            data[index++] = questData;
        }

        return data;
    }


    public void LoadQuest(QuestData[] data)
    {
        for(int i = 0; i < data.Length; i++)
        {
            Quest quest = questMap[data[i].questId];
            QuestStepData[][] stepData = quest.Convert2DArrayFrom1DArray(data[i].questStepData);
            quest.LoadQuestData(data[i].state, data[i].currentQuestStepIndex, stepData);

            if (quest.state == QuestState.IN_PROGRESS || quest.state == QuestState.CAN_FINISH)
            {
                quest.InstantiateLoadedQuestStep(this.transform);
            }

            if (quest.state == QuestState.FINISHED)
            {
                NotifyQuestFinished(quest);
            }

            NotifyQuestStateToQuestPoints(quest);
        }

    }
}
