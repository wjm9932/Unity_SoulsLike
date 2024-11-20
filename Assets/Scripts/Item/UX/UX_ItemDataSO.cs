using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Create New UX_ItemSO")]
public class UX_ItemDataSO : ScriptableObject
{
    public GameObject icon;
    public float dropChance;
    public string itemName;
}
