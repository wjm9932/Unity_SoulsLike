using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;

    private void Awake()
    {
        if (GameDataSaveLoadManager.Instance.FileExist() == false)
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
        SceneLoadManager.Instance.LoadScene("GameScene", () => GameDataSaveLoadManager.Instance.Initialize());
    }

    public void ContinueGame()
    {
        SceneLoadManager.Instance.LoadScene("GameScene", ()=>GameDataSaveLoadManager.Instance.Load());
    }

    public void Exit()
    {
        Application.Quit();
    }
}
