using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class UI_Book : UI_Item, UsableItem
{
    [Header("Item Info")]
    [SerializeField] private ItemData dataField;

    public ItemData data
    {
        get { return dataField; }
    }
    public event Action OnUseItem;

    public bool UseItem(LivingEntity livingEntity)
    {
        --count;

        if (count <= 0)
        {
            DestroyItem();
        }

        if (OnUseItem != null)
        {
            OnUseItem();
        }

        return true;
    }

    public IState GetTargetState(PlayerMovementStateMachine stateMachine)
    {
        return stateMachine.increaseMaxHpState;
    }
}
