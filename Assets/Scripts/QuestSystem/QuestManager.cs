using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public event Action<Quest> onChangeQuestState;
    public event Func<bool> onInteractWithQuest;

    [SerializeField]
    private Character questOwner;

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
        foreach(Quest quest in questMap.Values)
        {
            NotifyQuestStateToQuestPoints(quest);
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
        //Debug.Log("Start Quest: " + id);

        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest, QuestState.IN_PROGRESS);
    }

    public void AdvanceQuest(string id)
    {
        Debug.Log("Advance Quest: " + id);

        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();

        if(quest.CurrentStepExists() == true)
        {
            quest.DestroyCurrentQuestStep();
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest, QuestState.CAN_FINISH);
        }
    }

    public void FinishQuest(string id)
    {
        Debug.Log("Finish Quest: " + id);

        Quest quest = GetQuestById(id);

        //if(quest.GetReward(quest.info.rewardItem, quest.info.rewardItemCount) == false)
        //{
        //    return;
        //}

        quest.DestroyCurrentQuestStep();
        ChangeQuestState(quest, QuestState.FINISHED);
        FindCanStartQuest();
    }

    private void FindCanStartQuest()
    {
        foreach(Quest quest in questMap.Values)
        {
            if(IsRequirementMet(quest) == true && quest.state == QuestState.REQUIREMENTS_NOT_MET)
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
        quest.ChangeQuestState(state);
        NotifyQuestStateToQuestPoints(quest);
    }


    public void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.ChangeQuestState(state);
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
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }
}
