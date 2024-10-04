using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UX
{
    public class ItemBone : Item
    {
        private void Awake()
        {
            triggerCount = 0;
            numOfItem = 1;
            itemName = "Bone!";
        }
    }
}


