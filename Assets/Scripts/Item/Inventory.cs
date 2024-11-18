using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    [SerializeField]
    private List<GameObject> inventorySlot = new List<GameObject>(36);
    private Dictionary<string, UI.UI_Item> itemContainer = new Dictionary<string, UI.UI_Item>(36);
    private PointerEventData pointerEventData;
    private PlayerEvent playerEvent;

    private void Awake()
    {
        playerEvent = GetComponent<PlayerEvent>();
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

    public bool HasEnoughSpace(UX.UX_Item item)
    {
        if (itemContainer.ContainsKey(item.itemName) == false)
        {
            if (FindEmptySlot(item) == -1)
            {
                return false;
            }
        }

        return true;
    }

    public bool AddItem(UX.UX_Item item, int count)
    {
        if (item == null)
        {
            Debug.LogError("targetItem has no UX.UX_Item Component");
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

                UI.UI_Item inventoryItem = Instantiate(item.icon, inventorySlot[emptySlot].transform).GetComponent<UI.UI_Item>();
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

    public int GetTargetItemCountFromInventory(UX.UX_Item targetItem)
    {
        if(targetItem == null)
        {
            Debug.LogError("targetItem has no UX.UX_Item Component");
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

    private int FindEmptySlot(UX.UX_Item item)
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

    public bool RemoveTargetItemFromInventory(UX.UX_Item targetItem, int count)
    {
        if (targetItem == null)
        {
            Debug.LogError("targetItem has no UX.UX_Item Component");
            return false;
        }
        
        if (itemContainer.ContainsKey(targetItem.itemName) == false)
        {
            return false;
        }

        UI.UI_Item item = itemContainer[targetItem.itemName].GetComponent<UI.UI_Item>();
        if (item.count < count)
        {
            return false;
        }
        else
        {
            item.count -= count;

            if (item.count <= 0)
            {
                RemoveItemFromInventory(item);
            }
        }

        return true;
    }

    private void RemoveItemFromInventory(UI.UI_Item item)
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

    private void DropItem(UI.UI_Item item, int count)
    {
        if (item == null)
        {
            Debug.LogError("targetItem has no UX.UX_Item Component");
            return;
        }

        UX.UX_Item items = Instantiate(item.itemUX, gameObject.transform.position, Quaternion.identity).GetComponent<UX.UX_Item>();
        items.triggerCount = 1;
        items.numOfItem = count;

        itemContainer[item.itemName].count = 0;
        playerEvent.UpdateItemCount(item.itemName);

        TextManager.Instance.PlayNotificationText("You've dropped " + items.itemName + " x" + count);
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    UX.UX_Item item = other.gameObject.GetComponent<UX.UX_Item>();

    //    if (item != null)
    //    {
    //        if (item.triggerCount <= 0)
    //        {
    //            if (AddItem(item, item.numOfItem) == true)
    //            {
    //                TextManager.Instance.PlayNotificationText("You've got " + item.itemName + " x" + item.numOfItem);
    //                Destroy(item.gameObject);
    //            }
    //            else
    //            {
    //                TextManager.Instance.PlayNotificationText(TextManager.DisplayText.INVENTORY_IS_FUll);
    //            }
    //        }
    //        else
    //        {
    //            --item.triggerCount;
    //        }
    //    }
    //}
}
