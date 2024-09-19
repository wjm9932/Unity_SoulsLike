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
        public bool UseItem(LivingEntity livingEntity)
        {
            if (livingEntity.IsMaxHp() == false)
            {
                --count;
                UpdateCount(count);

                if (count <= 0)
                {
                    DestroyItem(gameObject);
                    Destroy(gameObject);
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

