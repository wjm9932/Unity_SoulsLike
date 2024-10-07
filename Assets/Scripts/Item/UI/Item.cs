using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class Item : MonoBehaviour
    {
        public delegate void OnDestroyEventHandler(UI.Item item);
        public event OnDestroyEventHandler OnDestroy;

        public delegate void OnDropEventHandler(UI.Item item, int count);
        public event OnDropEventHandler OnDrop;

        public TextMeshProUGUI countText;

        [SerializeField]
        private GameObject _itemUX;
        public GameObject itemUX 
        {
            get { return _itemUX; }
        }
        private int _count = 0;

        public string itemName { get; private set; }
        public int count
        {
            set
            {
                _count = value;
                UpdateCount(_count);
            }
            get
            {
                return _count;
            }
        }

        public void UpdateCount(int count)
        {
            countText.text = count.ToString();
        }

        public void AddItem()
        {
            ++count;
            UpdateCount(count);
        }
        public void SetName(string name)
        {
            itemName = name;
        }
        public void DestroyItem()
        {
            OnDestroy?.Invoke(gameObject.GetComponent<UI.Item>());
        }

        public void DropItem()
        {
            OnDrop?.Invoke(gameObject.GetComponent<UI.Item>(), count);
            DestroyItem();
        }
    }
}
