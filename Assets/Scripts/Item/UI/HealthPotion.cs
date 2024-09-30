using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace UI
{
    public class HealthPotion : Item, UsableItem
    {
        [SerializeField]
        private ItemData dataField;

        public ItemData data
        {
            get { return dataField; }
        }
        public event Action OnUseItem;

        public bool UseItem(LivingEntity livingEntity)
        {
            if (livingEntity.IsMaxHp() == false)
            {
                --count;
                UpdateCount(count);
                
                if (count <= 0)
                {
                    DestroyItem();
                }

                if(OnUseItem != null)
                {
                    OnUseItem();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public IState GetTargetState(PlayerMovementStateMachine stateMachine)
        {
            return stateMachine.drinkPotionState;
        }
    }
}

