using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<ItemData> itemContainer = new List<ItemData>(15);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddItem(Item item)
    {
        itemContainer.Add(item.item);
    }
}
