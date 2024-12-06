using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject slotButtonPrefabs;
    [SerializeField] private GameObject slotButtonParent;
    [SerializeField] private GameObject slotListScrollView;

    private void Awake()
    {
        SetContinueButton();
    }

    public void StartNewGame()
    {
        SceneLoadManager.Instance.LoadScene("GameScene", () => GameDataSaveLoadManager.Instance.Initialize());
    }

    public void ContinueGame()
    {
        if (slotListScrollView.activeSelf == false)
        {
            slotListScrollView.SetActive(true);
        }
        else
        {
            slotListScrollView.SetActive(false);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void SetContinueButton()
    {
        string[] files = GameDataSaveLoadManager.Instance.GetAllSaveData();
        if (files.Length > 0)
        {
            continueButton.SetActive(true);
            CreateSlotButtons(files);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }
    private void CreateSlotButtons(string[] files)
    {
        float buttonHeight = slotButtonPrefabs.GetComponent<RectTransform>().rect.height;
        RectTransform slotButtonViewTransform = slotButtonParent.GetComponent<RectTransform>();
        slotButtonViewTransform.sizeDelta = new Vector2(slotButtonViewTransform.sizeDelta.x, slotButtonViewTransform.sizeDelta.y + buttonHeight * files.Length);

        for (int i = 0; i < files.Length; i++)
        {
            SlotData data = GameDataSaveLoadManager.Instance.GetSlotData(files[i]);
            var button = Instantiate(slotButtonPrefabs, slotButtonParent.transform).GetComponent<SlotButton>();
            button.Initialize(data.slotID, data.lastPlayDate, ConvertPlayTimeToString(data.totalPlayTime));
        }
    }

    private string ConvertPlayTimeToString(float playTime)
    {
        int hours = Mathf.FloorToInt(playTime / 3600);
        int minutes = Mathf.FloorToInt((playTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(playTime % 60);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
