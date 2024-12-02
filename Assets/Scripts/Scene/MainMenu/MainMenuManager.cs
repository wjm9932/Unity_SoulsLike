using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
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
    }
}
