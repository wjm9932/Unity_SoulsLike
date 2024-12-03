using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Character character;
    [SerializeField] private SceneGameDataManger sceneGameDataManger;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void Load()
    {
        string path = Path.Combine(Application.dataPath, "GameData");
        if (!File.Exists(path))

        {
            return;
        }

        string jsonData = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

        sceneGameDataManger.LoadSceneData(saveData.sceneSaveData);
        character.LoadData(saveData.playerData);
        character.inventory.LoadData(saveData.inventoryData);
        QuestManager.Instance.LoadQuest(saveData.questData);
    }


    public void Resume()
    {
        character.uiStateMachine.ChangeState(character.uiStateMachine.closeState);
    }
    public void Quit()
    {
        SaveGameData();

        SceneLoadManager.Instance.GoToMainMenu();
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void SaveGameData()
    {
        SaveData data = new SaveData();

        data.sceneSaveData = sceneGameDataManger.SaveSceneData();
        data.playerData = character.GetData();
        data.inventoryData = character.inventory.GetData();
        data.questData = QuestManager.Instance.GetData();

        string jsonData = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.dataPath, "GameData");
        File.WriteAllText(path, jsonData);
    }
}
