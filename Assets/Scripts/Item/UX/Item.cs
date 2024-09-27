using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UX
{
    public abstract class Item : MonoBehaviour
    {
        public GameObject icon;
        public int triggerCount;
        public string itemName { get; protected set; }
        public int numOfItem;
    }
}


