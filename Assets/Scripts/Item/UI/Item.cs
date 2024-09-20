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

        public TextMeshProUGUI countText;

        private int _count = 0;
        public int count
        {
            set
            {
                _count = value;
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

        public void DestroyItem(GameObject item)
        {
            OnDestroy?.Invoke(item);
            Destroy(item);
        }
    }
}
