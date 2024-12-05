using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class GameDataSaveLoadManager : MonoBehaviour
{
    public static GameDataSaveLoadManager Instance;

    private SceneGameDataManger sceneGameDataManger;
    private Character character;

    private int currentSlot;
    private string dataPath;
    private string[] saveData;

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

        saveData = Directory.GetFiles(dataPath, "*.json").OrderByDescending(File.GetLastWriteTime).ToArray();


        DontDestroyOnLoad(this.gameObject);
    }
    public void Initialize()
    {
        sceneGameDataManger = FindObjectOfType<SceneGameDataManger>();
        character = FindObjectOfType<Character>();
        currentSlot = saveData.Length;
    }
    public void Load(int slotID)
    {
        if (FileExist(slotID) == false)
        {
            Debug.LogError("There is no continue file!");
            return;
        }

        currentSlot = slotID;
        string path = Path.Combine(dataPath, "GameData" + slotID + ".json");
        string jsonData = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

        sceneGameDataManger = FindObjectOfType<SceneGameDataManger>();
        character = FindObjectOfType<Character>();

        sceneGameDataManger.LoadSceneData(saveData.sceneSaveData);
        character.LoadData(saveData.playerData);
        character.inventory.LoadData(saveData.inventoryData);
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

        string jsonData = JsonUtility.ToJson(data, true);
        string path = Path.Combine(dataPath, "GameData" + currentSlot + ".json");
        File.WriteAllText(path, jsonData);
    }
    public bool FileExist(int slotID)
    {
        string path = Path.Combine(dataPath, "GameData" + slotID + ".json");
        return File.Exists(path);
    }

    public string[] GetAllSaveData()
    {
        return saveData;
    }

    private SlotData GetData()
    {
        return new SlotData(currentSlot);
    }

    public SlotData GetSlotData(string dataPath)
    {
        string jsonData = File.ReadAllText(dataPath);
        return JsonUtility.FromJson<SaveData>(jsonData).slotData;
    }

    private void OnApplicationQuit()
    {
        if (character != null && sceneGameDataManger != null)
        {
            SaveGameData();
        }
    }
}
