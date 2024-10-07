using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UX
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField]
        private string _itemName;

        public string itemName
        {
            get { return _itemName; }
            private set { _itemName = value; }
        }
        public GameObject icon;
        public int triggerCount;
        public int numOfItem;
    }
}


