using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private Character player;

    private Dictionary<string, Quest> questMap;
    private void Awake()
    {
        questMap = CreateQuestMap();
        
        //Quest quest = GetQuestById("CollectHealthPotionQuest");
        //quest.InstantiateCurrentQuestStep(this.transform);
    }

    private void OnEnable()
    {
        player.GetComponent<EventManager>().questEvents.onStartQuest += StartQuest;
        player.GetComponent<EventManager>().questEvents.onAdvanceQuest += AdvanceQuest;
        player.GetComponent<EventManager>().questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        player.GetComponent<EventManager>().questEvents.onStartQuest -= StartQuest;
        player.GetComponent<EventManager>().questEvents.onAdvanceQuest -= AdvanceQuest;
        player.GetComponent<EventManager>().questEvents.onFinishQuest -= FinishQuest;
    }

    private void Start()
    {
        foreach(Quest quest in questMap.Values)
        {
            player.GetComponent<EventManager>().questEvents.ChangeQuestState(quest);
        }
    }

    private void StartQuest(string id)
    {
        Debug.Log("Start Quest: " + id);
    }

    private void AdvanceQuest(string id)
    {
        Debug.Log("Advance Quest: " + id);
    }

    private void FinishQuest(string id)
    {
        Debug.Log("Finish Quest: " + id);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        // loads all QuestInfoSO Scriptable Objects under the Assets/Resources/Quests folder
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        // Create the quest map
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo, player));
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
