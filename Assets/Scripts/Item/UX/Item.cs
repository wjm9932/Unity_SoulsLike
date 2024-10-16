using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UX
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField]
        private string _itemName;
        [SerializeField]
        private int _dropChace;
        public int dropChance { get { return _dropChace; } private set { _dropChace = value; } }
        public string itemName
        {
            get { return _itemName; }
            private set { _itemName = value; }
        }
        public GameObject icon;
        [HideInInspector]
        public int triggerCount;
        public int numOfItem = 1;
    }
}


