using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> inventorySlot = new List<GameObject>(35);

    private Dictionary<string, GameObject> itemContainer = new Dictionary<string, GameObject>(35);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(GameObject item)
    {
        if (itemContainer.ContainsKey(item.tag) == false)
        {
            for (int i = 0; i < inventorySlot.Count; i++)
            {
                if (inventorySlot[i].transform.childCount == 0)
                {
                    GameObject inventoryItem = Instantiate(item.GetComponent<UX.Item>().icon, inventorySlot[i].transform);
                    inventoryItem.GetComponent<UI.Item>().AddItem();
                    inventoryItem.GetComponent<UI.Item>().OnDestroy += RemoveItemFromInventory;

                    itemContainer.Add(item.tag, inventoryItem);

                    break;
                }
            }
        }
        else
        {
            itemContainer[item.tag].GetComponent<UI.Item>().AddItem();
        }
    }

    private void RemoveItemFromInventory(GameObject potion)
    {
        string itemTag = potion.gameObject.tag;

        if (itemContainer.ContainsKey(itemTag))
        {
            itemContainer.Remove(itemTag);
        }
    }
}
