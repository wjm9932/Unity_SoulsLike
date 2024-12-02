using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;

    private void Awake()
    {
        string path = Path.Combine(Application.dataPath, "GameData");
        if (!File.Exists(path))
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
    }
    public void StartNewGame()
    {
        SceneLoadManager.Instance.StartNewGame();
    }

    public void ContinueGame()
    {
        SceneLoadManager.Instance.ContinueGame(()=>GameManager.Instance.Load());
    }

    public void Exit()
    {
        Application.Quit();
    }
}
