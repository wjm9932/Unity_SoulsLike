using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    [Header("Save & Load")]
    [SerializeField] public bool allowLoad;
    
    [SerializeField] private List<GameObject> inventorySlot = new List<GameObject>(36);
    private Dictionary<string, UI_Item> itemContainer = new Dictionary<string, UI_Item>(36);
    private PointerEventData pointerEventData;
    private PlayerEvent playerEvent;

    private void Awake()
    {
        playerEvent = GetComponent<PlayerEvent>();
        LoadItem();
    }

    public UsableItem quickSlot
    {
        get
        {
            if (inventorySlot[0].transform.childCount == 0)
            {
                return null;
            }
            else
            {
                return inventorySlot[0].GetComponentInChildren<UsableItem>();
            }
        }
    }

    public bool HasEnoughSpace(UX_Item item)
    {
        if (itemContainer.ContainsKey(item.data.itemName) == false)
        {
            if (FindEmptySlot(item) == -1)
            {
                return false;
            }
        }

        return true;
    }

    public bool AddItem(UX_Item item, int count)
    {
        if (item == null)
        {
            Debug.LogError("targetItem has no UX.UX_Item Component");
            return false;
        }

        if (itemContainer.ContainsKey(item.data.itemName) == false)
        {
            int emptySlot = FindEmptySlot(item);

            if (emptySlot == -1)
            {
                return false;
            }

            UI_Item inventoryItem = Instantiate(item.data.icon, inventorySlot[emptySlot].transform).GetComponent<UI_Item>();
            inventoryItem.SetName(item.data.itemName);
            inventoryItem.count = count;
            inventoryItem.OnDestroy += RemoveItemFromItemContainer;
            inventoryItem.OnDrop += DropItem;

            if (inventoryItem.GetComponent<UsableItem>() != null)
            {
                inventoryItem.GetComponent<UsableItem>().OnUseItem += () => { playerEvent.UseItem(item.data.itemName); };
                inventoryItem.GetComponent<UsableItem>().OnUseItem += () => { playerEvent.UpdateItemCount(item.data.itemName); };
            }

            itemContainer.Add(item.data.itemName, inventoryItem);
        }
        else
        {
            itemContainer[item.data.itemName].count += count;
        }

        playerEvent.UpdateItemCount(item.data.itemName);
        return true;
    }

    public int GetTargetItemCountFromInventory(UX_Item targetItem)
    {
        if(targetItem == null)
        {
            Debug.LogError("targetItem has no UX.UX_Item Component");
            return -1;
        }
        if (itemContainer.ContainsKey(targetItem.data.itemName) == false)
        {
            return 0;
        }
        else
        {
            return itemContainer[targetItem.data.itemName].count;
        }
    }

    private int FindEmptySlot(UX_Item item)
    {
        for (int i = 0; i < inventorySlot.Count; i++)
        {
            if (inventorySlot[i].transform.childCount == 0)
            {
                if (i == 0 && item.data.icon.GetComponent<UsableItem>() == null)
                {
                    continue;
                }
                return i;
            }
        }
        return -1;
    }

    public bool RemoveTargetItemFromInventory(UX_Item targetItem, int count)
    {
        if (targetItem == null)
        {
            Debug.LogError("targetItem has no UX.UX_Item Component");
            return false;
        }
        
        if (itemContainer.ContainsKey(targetItem.data.itemName) == false)
        {
            return false;
        }

        UI_Item item = itemContainer[targetItem.data.itemName].GetComponent<UI_Item>();
        if (item.count < count)
        {
            return false;
        }
        else
        {
            item.count -= count;

            if (item.count <= 0)
            {
                RemoveItemFromItemContainer(item);
            }
        }

        return true;
    }

    private void RemoveItemFromItemContainer(UI_Item item)
    {
        if (item == null)
        {
            Debug.LogError("targetItem has no UX.UX_Item Component");
            return;
        }

        if (itemContainer.ContainsKey(item.itemName) == true)
        {
            itemContainer.Remove(item.itemName);
            Destroy(item.gameObject);
        }
    }

    private void DropItem(UI_Item item, int count)
    {
        if (item == null)
        {
            Debug.LogError("targetItem has no UX.UX_Item Component");
            return;
        }

        UX_Item items = Instantiate(item.itemUX, gameObject.transform.position, Quaternion.identity).GetComponent<UX_Item>();
        items.triggerCount = 1;
        items.numOfItem = count;

        itemContainer[item.itemName].count = 0;
        playerEvent.UpdateItemCount(item.itemName);

        TextManager.Instance.PlayNotificationText("You've dropped " + items.data.itemName + " x" + count);
    }
    public UsableItem GetItemUI()
    {
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();

        uiRaycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            GameObject item = results[0].gameObject;
            
            UI_Item uiItem = item.GetComponent<UI_Item>();
            if(uiItem != null)
            {
                SoundManager.Instance.Play2DSoundEffect(SoundManager.UISoundEffectType.CLICK, 0.15f);
            }

            UsableItem usableItem = item.GetComponent<UsableItem>();
            if (usableItem != null)
            {
                return usableItem;
            }
        }

        return null;
    }

    private void OnApplicationQuit()
    {

    }

    public void SaveInventory()
    {
        List<ItemData> data = new List<ItemData>();

        for (int i = 0; i < inventorySlot.Count; i++)
        {
            if (inventorySlot[i].transform.childCount != 0)
            {
                UI_Item item = inventorySlot[i].GetComponentInChildren<UI_Item>();
                data.Add(new ItemData(i, item.itemName, item.count));
            }
        }
        InventoryData inventoryData = new InventoryData(data);

        string jsonData = JsonUtility.ToJson(inventoryData, true);
        string path = Path.Combine(Application.dataPath, "InvetoryData");
        File.WriteAllText(path, jsonData);
    }

    public InventoryData GetData()
    {
        List<ItemData> data = new List<ItemData>();

        for (int i = 0; i < inventorySlot.Count; i++)
        {
            if (inventorySlot[i].transform.childCount != 0)
            {
                UI_Item item = inventorySlot[i].GetComponentInChildren<UI_Item>();
                data.Add(new ItemData(i, item.itemName, item.count));
            }
        }
        InventoryData inventoryData = new InventoryData(data);

        return inventoryData;
    }

    public void LoadItem()
    {
        if(allowLoad == false)
        {
            return;
        }

        string path = Path.Combine(Application.dataPath, "InvetoryData");

        if (!File.Exists(path))
        {
            Debug.LogWarning("No inventory data found. Starting with an empty inventory.");
            return;
        }

        string jsonData = File.ReadAllText(path);
        InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(jsonData);

        UX_ItemDataSO[] itemPrefabs = Resources.LoadAll<UX_ItemDataSO>("Item");
        Dictionary<string, UX_ItemDataSO> map = new Dictionary<string, UX_ItemDataSO>();

        for(int i = 0; i < itemPrefabs.Length; i++)
        {
            map.Add(itemPrefabs[i].itemName, itemPrefabs[i]);
        }

        for(int i = 0; i < inventoryData.itemData.Count; i++)
        {
            UX_ItemDataSO item = map[inventoryData.itemData[i].itemName];
            LoadItemToInventory(item, inventoryData.itemData[i].itemCount, inventoryData.itemData[i].slotIndex);
        }
    }

    public void LoadData(InventoryData data)
    {
        UX_ItemDataSO[] itemPrefabs = Resources.LoadAll<UX_ItemDataSO>("Item");
        Dictionary<string, UX_ItemDataSO> map = new Dictionary<string, UX_ItemDataSO>();

        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            map.Add(itemPrefabs[i].itemName, itemPrefabs[i]);
        }

        for (int i = 0; i < data.itemData.Count; i++)
        {
            UX_ItemDataSO item = map[data.itemData[i].itemName];
            LoadItemToInventory(item, data.itemData[i].itemCount, data.itemData[i].slotIndex);
        }
    }

    private void LoadItemToInventory(UX_ItemDataSO item, int count, int slot)
    {
        UI_Item inventoryItem = Instantiate(item.icon, inventorySlot[slot].transform).GetComponent<UI_Item>();
        inventoryItem.count = count;

        inventoryItem.SetName(item.itemName);
        inventoryItem.OnDestroy += RemoveItemFromItemContainer;
        inventoryItem.OnDrop += DropItem;

        if (inventoryItem.GetComponent<UsableItem>() != null)
        {
            inventoryItem.GetComponent<UsableItem>().OnUseItem += () => { playerEvent.UpdateItemCount(item.itemName); };
            inventoryItem.GetComponent<UsableItem>().OnUseItem += () => { playerEvent.UseItem(item.itemName); };
        }

        itemContainer.Add(item.itemName, inventoryItem);
    }
}
