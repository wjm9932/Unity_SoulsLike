using System.Collections;
using System.Collections.Generic;
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
}
