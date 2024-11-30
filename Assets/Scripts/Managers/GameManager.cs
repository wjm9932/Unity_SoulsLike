using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Character character;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        
    }

    public void Load()
    {
        character.LoadData();
        character.inventory.LoadItem();

    }

    public void Quit()
    {
        character.SaveData();
        character.inventory.SaveInventory();
        QuestManager.Instance.SaveQuest();

        SaveGameData();
        
        SceneLoadManager.Instance.GoToMainMenu();
    }

    public void Resume()
    {
        character.uiStateMachine.ChangeState(character.uiStateMachine.closeState);
    }

    private void OnApplicationQuit()
    {
        character.SaveData();
        character.inventory.SaveInventory();
        QuestManager.Instance.SaveQuest();
    }

    private void SaveGameData()
    {
        SaveData data = new SaveData();

        data.playerData = character.GetData();
        data.inventoryData = character.inventory.GetData();
        data.questData = QuestManager.Instance.GetData();

        string jsonData = JsonUtility.ToJson(data, true);
        string path = Path.Combine(Application.dataPath, "GameData");
        File.WriteAllText(path, jsonData);
    }
}
