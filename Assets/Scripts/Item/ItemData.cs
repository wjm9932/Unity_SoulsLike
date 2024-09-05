using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Create New Item")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Potion,
        Weapon,
    }

    public ItemType type;
    public int value;
    public Sprite icon;
}
