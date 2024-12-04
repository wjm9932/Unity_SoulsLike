using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuestManager : MonoBehaviour
{
    [Header("QuestUI")]
    public GameObject questLogUI;
    public GameObject questDialogueUI;

    [Header("Quests")]
    [SerializeField] private QuestInfoSO skillIsOnQuest;
    public bool isSkillOn { get; private set; }

    private void Awake()
    {
        isSkillOn = false;
    }
    public void KillLivingEntity(Character character, EntityType type)
    {
        character.playerEvents.KillEnemy(type);
    }
    private void EnableSkill(Quest quest)
    {
        if (quest.info.id == skillIsOnQuest.id)
        {
            isSkillOn = true;
            QuestManager.Instance.onFinishQuest -= EnableSkill;
        }
    }

    private void OnEnable()
    {
        QuestManager.Instance.onFinishQuest += EnableSkill;
    }

    private void OnDisable()
    {
        QuestManager.Instance.onFinishQuest -= EnableSkill;
    }
}
