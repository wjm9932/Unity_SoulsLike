using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    [SerializeField]
    private List<GameObject> inventorySlot = new List<GameObject>(36);

    private Dictionary<string, GameObject> itemContainer = new Dictionary<string, GameObject>(36);
    private PointerEventData pointerEventData;

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
    public void AddItem(UX.Item item)
    {
        int emptySlot = FindEmptySlot(item);

        if(emptySlot == -1)
        {
            TextManager.Instance.PlayNotificationText(TextManager.DisplayText.INVENTORY_IS_FUll);
        }
        else
        {
            if (itemContainer.ContainsKey(item.tag) == false)
            {
                GameObject inventoryItem = Instantiate(item.GetComponent<UX.Item>().icon, inventorySlot[emptySlot].transform);
                inventoryItem.transform.tag = item.tag;
                inventoryItem.GetComponent<UI.Item>().AddItem();
                inventoryItem.GetComponent<UI.Item>().OnDestroy += RemoveItemFromInventory;
                inventoryItem.GetComponent<UI.Item>().OnDrop += DropItem;

                itemContainer.Add(item.tag, inventoryItem);
            }
            else
            {
                itemContainer[item.tag].GetComponent<UI.Item>().AddItem();
            }

            Destroy(item.gameObject); 
        }
    }

    private int FindEmptySlot(UX.Item item)
    {
        for (int i = 0; i < inventorySlot.Count; i++)
        {
            if (inventorySlot[i].transform.childCount == 0)
            {
                if (i == 0 && item.GetComponent<UX.Item>().icon.GetComponent<UsableItem>() == null)
                {
                    continue;
                }
                return i;
            }
        }
        return -1;
    }

    private void RemoveItemFromInventory(GameObject item)
    {
        if (itemContainer.ContainsKey(item.gameObject.tag) == true)
        {
            itemContainer.Remove(item.gameObject.tag);
        }
    }
    private void DropItem(GameObject item, int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject items = Instantiate(item, gameObject.transform.position, Quaternion.identity);
            items.GetComponent<UX.HealthPotion>().triggerCount = 1;
        }
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
}
