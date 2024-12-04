using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryData
{
    public List<ItemData> itemData;

    public InventoryData(List<ItemData> data)
    {
        itemData = data;
    }
}
