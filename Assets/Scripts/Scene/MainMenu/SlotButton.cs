using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotButton : MonoBehaviour
{
    private int slotID;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(LoadData);
    }

    public void SetSlotID(int slotID)
    {
        this.slotID = slotID;
    }

    public void LoadData()
    {
        SceneLoadManager.Instance.LoadScene("GameScene", () => GameDataSaveLoadManager.Instance.Load(slotID));
    }
}
