using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataSaveLoadManager : MonoBehaviour
{
    public static GameDataSaveLoadManager Instance;

    private SceneGameDataManger sceneGameDataManger;
    private Character character;

    private int currentSlot;
    private string dataPath;
    public int numOfData { get; private set; }


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
    }
    public void Load()
    {
        if (FileExist() == false)
        {
            Debug.LogError("There is no continue file!");
            return;
        }

        string path = Path.Combine(dataPath, "GameData");
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

        data.sceneSaveData = sceneGameDataManger.SaveSceneData();
        data.playerData = character.GetData();
        data.inventoryData = character.inventory.GetData();
        data.questData = QuestManager.Instance.GetData();
        data.soundSettingData = SoundManager.Instance.GetData();

        string jsonData = JsonUtility.ToJson(data, true);
        string path = Path.Combine(dataPath, "GameData");
        File.WriteAllText(path, jsonData);
    }
    public bool FileExist()
    {
        string path = Path.Combine(dataPath, "GameData");
        return File.Exists(path);
    }

    public void SetCurrentSlot(int slotId)
    {
        currentSlot = slotId;
    }
    public int GetCurrentSlot()
    {
        return currentSlot;
    }
    
    private void OnApplicationQuit()
    {
        if(character != null && sceneGameDataManger != null)
        {
            SaveGameData();
        }
    }
}
