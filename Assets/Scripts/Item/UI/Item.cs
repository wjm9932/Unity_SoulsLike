using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class Item : MonoBehaviour
    {
        public delegate void OnDestroyEventHandler(GameObject item);
        public event OnDestroyEventHandler OnDestroy;

        public delegate void OnDropEventHandler(GameObject item, int count);
        public event OnDropEventHandler OnDrop;

        public TextMeshProUGUI countText;

        [SerializeField]
        private GameObject itemUX;

        private int _count = 0;
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

        public void DestroyItem()
        {
            OnDestroy?.Invoke(gameObject);
        }

        public void DropItem()
        {
            OnDrop?.Invoke(itemUX, count);
            DestroyItem();
        }
    }
}
