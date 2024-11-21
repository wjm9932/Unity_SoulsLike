using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemData
{
    public int slotIndex;
    public string itemName;
    public int itemCount;

    public ItemData(int slotIndex, string itemName, int itemCount)
    {
        this.slotIndex = slotIndex;
        this.itemName = itemName;
        this.itemCount = itemCount;
    }
}
