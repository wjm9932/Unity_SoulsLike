using System;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class UI_ArmorBuffPotion : UI_Item, UsableItem
{
    [Header("Item Info")]
    [SerializeField] private ItemDataSO dataField;

    public ItemDataSO data
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
        return stateMachine.drinkArmorBuffPotionState;
    }
}
