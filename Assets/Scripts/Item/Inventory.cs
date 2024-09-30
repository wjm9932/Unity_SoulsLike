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

    private EventManager playerEvent;

    private void Awake()
    {
        playerEvent = GetComponent<EventManager>();
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


    public bool AddItem(UX.Item item, int count)
    {
        int emptySlot = FindEmptySlot(item);

        if (emptySlot == -1)
        {
            return false;
        }

        for (int i = 0; i < count; i++)
        {
            if (itemContainer.ContainsKey(item.tag) == false)
            {
                GameObject inventoryItem = Instantiate(item.GetComponent<UX.Item>().icon, inventorySlot[emptySlot].transform);
                inventoryItem.transform.tag = item.tag;
                inventoryItem.GetComponent<UI.Item>().AddItem();
                inventoryItem.GetComponent<UI.Item>().OnDestroy += RemoveItemFromInventory;
                inventoryItem.GetComponent<UI.Item>().OnDrop += DropItem;
                
                if(inventoryItem.GetComponent<UsableItem>() != null)
                {
                    inventoryItem.GetComponent<UsableItem>().OnUseItem += playerEvent.UpdateItemCount; 
                }

                itemContainer.Add(item.tag, inventoryItem);
            }
            else
            {
                itemContainer[item.tag].GetComponent<UI.Item>().AddItem();
            }
        }

        //playerEvent.UpdateItemCount();
        //and in quest script, lets assume the getPotionQuest, playerEvent.onCollect += CheckHealthPotionCount; CheckHealthPotionCount() : count = owner.inventory.FindItem("HealthPotion"); if(count >= target) FinishQeust();

        Destroy(item.gameObject);
        return true;
    }
    
    public int FindItem(string item)
    {
        if (itemContainer.ContainsKey(item) == false)
        {
            return 0;
        }
        else
        {
            return itemContainer[item].GetComponent<UI.Item>().count;
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
        GameObject items = Instantiate(item, gameObject.transform.position, Quaternion.identity);
        items.GetComponent<UX.Item>().triggerCount = 1;
        items.GetComponent<UX.Item>().numOfItem = count;

        //itemContainer[item.tag].GetComponent<UI.Item>().count = 0;
        //playerEvent.UpdateItemCount();

        TextManager.Instance.PlayNotificationText("You've dropped " + items.GetComponent<UX.Item>().itemName + "x" + count);
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
