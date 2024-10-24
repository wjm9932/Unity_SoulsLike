using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;


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
        [SerializeField]
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
        private void OnTriggerEnter(Collider other)
        {
            Character character = other.GetComponent<Character>();
            if(character != null)
            {
                if(triggerCount  <= 0)
                {
                    if(character.inventory.AddItem(this, numOfItem) == true)
                    {
                        TextManager.Instance.PlayNotificationText("You've got " + itemName + " x" + numOfItem);
                        Destroy(this.gameObject);
                    }
                    else
                    {
                        TextManager.Instance.PlayNotificationText(TextManager.DisplayText.INVENTORY_IS_FUll);
                    }
                }
                else
                {
                    --triggerCount;
                }
            }
        }
    }
}


