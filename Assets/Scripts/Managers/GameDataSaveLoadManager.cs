using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;

public class GameDataSaveLoadManager : MonoBehaviour
{
    public static GameDataSaveLoadManager Instance;

    private SceneGameDataManger sceneGameDataManger;
    private Character character;

    private string currentSlot;
    private string dataPath;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        dataPath = Path.Combine(Application.dataPath, "Save Data");
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    public void Initialize()
    {
        sceneGameDataManger = FindObjectOfType<SceneGameDataManger>();
        character = FindObjectOfType<Character>();
        currentSlot = Guid.NewGuid().ToString();
    }
    public void Load(string slotID)
    {
        if (FileExist(slotID) == false)
        {
            Debug.LogError("There is no continue file!");
            return;
        }

        currentSlot = slotID;
        string path = Path.Combine(dataPath, slotID);
        string jsonData = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

        sceneGameDataManger = FindObjectOfType<SceneGameDataManger>();
        character = FindObjectOfType<Character>();

        sceneGameDataManger.LoadSceneData(saveData.sceneSaveData);
        character.LoadData(saveData.playerData);
        character.inventory.LoadData(saveData.inventoryData);
        PlayTimeTracker.LoadData(saveData.slotData);
        QuestManager.Instance.LoadQuest(saveData.questData);
        SoundManager.Instance.LoadSoundData(saveData.soundSettingData);
    }
    public void SaveGameData()
    {
        SaveData data = new SaveData();

        data.slotData = GetData();
        data.sceneSaveData = sceneGameDataManger.SaveSceneData();
        data.playerData = character.GetData();
        data.inventoryData = character.inventory.GetData();
        data.questData = QuestManager.Instance.GetData();
        data.soundSettingData = SoundManager.Instance.GetData();

        string path = Path.Combine(dataPath, currentSlot);
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, jsonData);
    }
    public bool FileExist(string slotID)
    {
        string path = Path.Combine(dataPath, slotID);
        return File.Exists(path);
    }

    public string[] GetAllSaveData()
    {
        return Directory.GetFiles(dataPath).OrderByDescending(File.GetLastWriteTime).ToArray();
    }

    private SlotData GetData()
    {
        return new SlotData(DateTime.Now.ToString(("yyyy-MM-dd hh:mm tt")), PlayTimeTracker.GetTotalPlayTime());
    }

    public SlotData GetSlotData(string dataPath)
    {
        string jsonData = File.ReadAllText(dataPath);
        return JsonUtility.FromJson<SaveData>(jsonData).slotData;
    }

    public void DeleteSaveData(string slotID)
    {
        if(FileExist(slotID) == false)
        {
            Debug.LogError("There is no data to be deleted");
            return;
        }

        string path = Path.Combine(dataPath, slotID);
        File.Delete(path);
    }

    private void OnApplicationQuit()
    {
        if (character != null && sceneGameDataManger != null)
        {
            SaveGameData();
        }
    }
}
