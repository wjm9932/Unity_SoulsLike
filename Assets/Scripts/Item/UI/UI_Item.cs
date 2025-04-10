using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class UI_Item : MonoBehaviour
{
    public delegate void OnDestroyEventHandler(UI_Item item);
    public event OnDestroyEventHandler OnDestroy;

    public delegate void OnDropEventHandler(UI_Item item, int count);
    public event OnDropEventHandler OnDrop;

    [Space]
    [SerializeField] private GameObject _itemUX;
    [SerializeField] private TextMeshProUGUI countText;

    [Header("Tool Tip Text")]
    [TextArea(3, 10)]
    [SerializeField] public string toolTipText;

    
    public string itemName { get; private set; }
    public GameObject itemUX
    {
        get { return _itemUX; }
    }
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
    public void SetName(string name)
    {
        itemName = name;
    }
    public void DestroyItem()
    {
        OnDestroy?.Invoke(gameObject.GetComponent<UI_Item>());
        ToolTipManager.Instance.HideToolTip();
    }

    public void DropItem()
    {
        OnDrop?.Invoke(gameObject.GetComponent<UI_Item>(), count);
        DestroyItem();
    }
}
