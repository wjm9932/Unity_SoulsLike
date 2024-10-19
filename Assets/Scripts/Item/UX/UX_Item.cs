using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UX
{
    public class UX_Item : MonoBehaviour
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
        [HideInInspector]
        public int numOfItem;


        void Awake()
        {
            triggerCount = 0;
            numOfItem = 1;
        }
    }

}


