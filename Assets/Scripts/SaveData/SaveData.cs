using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SaveData 
{
    public SceneSaveData sceneSaveData;
    public PlayerSaveData playerData;
    public InventoryData inventoryData;
    public QuestData[] questData;
    public SoundSettingData soundSettingData;
}

[System.Serializable]
public struct SceneData
{
    public string itemName;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public int count;
}


[System.Serializable]
public struct SceneSaveData
{
    public List<SceneData> sceneData;

    public SceneSaveData(List<SceneData> data)
    {
        sceneData = data;
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public float currentHealth;
    public float maxHealth;
    public float currentStamina;
    public AreaType musicType;
    public Vector3 checkPoint;

    public PlayerSaveData(Vector3 playerPosition, Quaternion playerRotation, float currentHealth, float maxHealth, float currentStamina, AreaType musicType, Vector3 checkPoint)
    {
        this.playerPosition = playerPosition;
        this.playerRotation = playerRotation;
        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
        this.currentStamina = currentStamina;
        this.musicType = musicType;
        this.checkPoint = checkPoint;
    }
}

[System.Serializable]
public struct QuestData
{
    public string questId;
    public QuestState state;
    public int currentQuestStepIndex;
    public QuestStepData[] questStepData;

    public QuestData(string questId, QuestState state, int currentQuestStepIndex, QuestStepData[] questStepData)
    {
        this.questId = questId;
        this.state = state;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepData = questStepData;
    }
}

[System.Serializable]
public class QuestStepData
{
    public QuestStepState questStepState;
    public string status;
    public string count;

    public QuestStepData()
    {
        this.questStepState = QuestStepState.IN_PROGRESS;
        this.status = "";
        this.count = "";
    }
}


[System.Serializable]
public struct ItemData
{
    public int slotIndex;
    public string itemName;
    public int itemCount;

    public ItemData(int slotIndex, string itemName, int itemCount)
    {
        this.slotIndex = slotIndex;
        this.itemName = itemName;
        this.itemCount = itemCount;
    }
}

[System.Serializable]
public struct InventoryData
{
    public List<ItemData> itemData;

    public InventoryData(List<ItemData> data)
    {
        itemData = data;
    }
}

[System.Serializable]
public struct SoundSettingData
{
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public SoundSettingData(float masterVolume, float bgmVolume, float sfxVolume)
    {
        this.masterVolume = masterVolume;
        this.bgmVolume = bgmVolume;
        this.sfxVolume = sfxVolume;
    }
}
