using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lastPlayedDateText;
    [SerializeField] private TextMeshProUGUI totalplayTimeText;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;

    private string slotID;

    public void Initialize(string slotID, string lastPlayedData, string totalPlayTime)
    {
        this.slotID = slotID;
        lastPlayedDateText.text = lastPlayedData;
        totalplayTimeText.text = totalPlayTime;
        loadButton.onClick.AddListener(LoadData);
        deleteButton.onClick.AddListener(DeleteData);
    }

    public void LoadData()
    {
        SceneLoadManager.Instance.LoadScene("GameScene", () => GameDataSaveLoadManager.Instance.Load(slotID));
    }
    public void DeleteData()
    {
        GameDataSaveLoadManager.Instance.DeleteSaveData(slotID);
        Destroy(gameObject);
    }
}
