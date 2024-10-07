using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    [SerializeField]
    private List<GameObject> inventorySlot = new List<GameObject>(36);
    private Dictionary<string, UI.Item> itemContainer = new Dictionary<string, UI.Item>(36);
    private PointerEventData pointerEventData;
    private PlayerQuestEvent playerEvent;

    private void Awake()
    {
        playerEvent = GetComponent<PlayerQuestEvent>();
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

    public bool HasEnoughSpace(UX.Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (itemContainer.ContainsKey(items[i].itemName) == false)
            {
                if (FindEmptySlot(items[i]) == -1)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool AddItem(UX.Item item, int count)
    {
        if (item == null)
        {
            Debug.LogError("targetItem has no UX.Item Component");
            return false;
        }

        for (int i = 0; i < count; i++)
        {
            if (itemContainer.ContainsKey(item.itemName) == false)
            {
                int emptySlot = FindEmptySlot(item);

                if (emptySlot == -1)
                {
                    return false;
                }

                UI.Item inventoryItem = Instantiate(item.icon, inventorySlot[emptySlot].transform).GetComponent<UI.Item>();
                inventoryItem.AddItem();
                inventoryItem.SetName(item.itemName);
                inventoryItem.OnDestroy += RemoveItemFromInventory;
                inventoryItem.OnDrop += DropItem;

                if (inventoryItem.GetComponent<UsableItem>() != null)
                {
                    inventoryItem.GetComponent<UsableItem>().OnUseItem += () => { playerEvent.UpdateItemCount(item.itemName); };
                    inventoryItem.GetComponent<UsableItem>().OnUseItem += () => { playerEvent.UseItem(item.itemName); };
                }

                itemContainer.Add(item.itemName, inventoryItem);
            }
            else
            {
                itemContainer[item.itemName].AddItem();
            }
        }

        playerEvent.UpdateItemCount(item.itemName);
        return true;
    }

    public int GetTargetItemCountFromInventory(UX.Item targetItem)
    {
        if(targetItem == null)
        {
            Debug.LogError("targetItem has no UX.Item Component");
            return -1;
        }
        if (itemContainer.ContainsKey(targetItem.itemName) == false)
        {
            return 0;
        }
        else
        {
            return itemContainer[targetItem.itemName].count;
        }
    }

    private int FindEmptySlot(UX.Item item)
    {
        for (int i = 0; i < inventorySlot.Count; i++)
        {
            if (inventorySlot[i].transform.childCount == 0)
            {
                if (i == 0 && item.icon.GetComponent<UsableItem>() == null)
                {
                    continue;
                }
                return i;
            }
        }
        return -1;
    }

    public bool RemoveTargetItemFromInventory(UX.Item targetItem, int count)
    {
        if (targetItem == null)
        {
            Debug.LogError("targetItem has no UX.Item Component");
            return false;
        }

        UI.Item item = itemContainer[targetItem.itemName].GetComponent<UI.Item>();
        if (item.count < count)
        {
            return false;
        }
        else
        {
            if (item.count <= 0)
            {
                RemoveItemFromInventory(item);
            }
            else
            {
                item.count -= count;
            }
        }

        return true;
    }

    private void RemoveItemFromInventory(UI.Item item)
    {
        if (item == null)
        {
            Debug.LogError("targetItem has no UX.Item Component");
            return;
        }

        if (itemContainer.ContainsKey(item.itemName) == true)
        {
            itemContainer.Remove(item.itemName);
            Destroy(item.gameObject);
        }
    }

    private void DropItem(UI.Item item, int count)
    {
        if (item == null)
        {
            Debug.LogError("targetItem has no UX.Item Component");
            return;
        }

        UX.Item items = Instantiate(item.itemUX, gameObject.transform.position, Quaternion.identity).GetComponent<UX.Item>();
        items.triggerCount = 1;
        items.numOfItem = count;

        itemContainer[item.itemName].count = 0;
        playerEvent.UpdateItemCount(item.itemName);

        TextManager.Instance.PlayNotificationText("You've dropped " + items.itemName + "x" + count);
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
            UsableItem item = results[0].gameObject.GetComponent<UsableItem>();
            if (item != null)
            {
                return item;
            }
        }

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        UX.Item item = other.gameObject.GetComponent<UX.Item>();

        if (item != null)
        {

            if (item.triggerCount <= 0)
            {
                if (AddItem(item, item.numOfItem) == true)
                {
                    TextManager.Instance.PlayNotificationText("You've got " + item.itemName + " x" + item.numOfItem);
                    Destroy(item.gameObject);
                }
                else
                {
                    TextManager.Instance.PlayNotificationText(TextManager.DisplayText.INVENTORY_IS_FUll);
                }
            }
            else
            {
                --item.triggerCount;
            }
        }
    }
}
