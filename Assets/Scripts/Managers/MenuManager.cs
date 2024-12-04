using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Menu")]
    public GameObject pauseMenu;
    public GameObject menuList;
    public GameObject settingMenu;


    public GameObject respawnMenu;

    [SerializeField] private Character character;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    public void Quit()
    {
        GameManager.Instance.SaveGameData();
        SceneLoadManager.Instance.GoToMainMenu();
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        character.uiStateMachine.ChangeState(character.uiStateMachine.closeState);
        SoundManager.Instance.Play2DSoundEffect(SoundManager.UISoundEffectType.CLICK, 0.3f);
    }

    public void Setting()
    {
        character.uiStateMachine.ChangeState(character.uiStateMachine.openSettingMenu);
    }

}
