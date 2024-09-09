using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace UI
{
    public class HealthPotion : Item
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public override bool UseItem(LivingEntity livingEntity)
        {
            if(livingEntity.RecoverHP(data.value) == true)
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
    }
}

