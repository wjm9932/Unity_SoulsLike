using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;
    private Action loadAction;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void StartNewGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ContinueGame(Action action)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadAction = action;

        SceneManager.LoadScene("GameScene");
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == "GameScene")
        {

        }
    }
}
