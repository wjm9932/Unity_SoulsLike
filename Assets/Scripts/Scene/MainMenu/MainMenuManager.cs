using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject slotButtonPrefabs;
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
        for (int i = 0; i < files.Length; i++)
        {
            SlotData data = GameDataSaveLoadManager.Instance.GetSlotData(files[i]);
            //var button = Instantiate(slotButtonPrefabs, slotListScrollView.transform).GetComponent<SlotButton>();
            //button.SetSlotID(data.slotID);
        }
    }
}
