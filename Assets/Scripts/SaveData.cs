using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SaveData 
{
    public PlayerSaveData playerData;
    public InventoryData inventoryData;
    public QuestData[] questData;
}
