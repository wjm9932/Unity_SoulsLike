using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    private Transform _originParent;
    public Transform originParent
    { 
        set 
        {
            _originParent = value;
        } 
        get
        {
            return _originParent;
        }
    }

    private Image image;
    private Vector3 offset;
    private UI.UI_Item item;

    private void Awake()
    {
        image = GetComponent<Image>();
        item = GetComponent<UI.UI_Item>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _originParent = transform.parent;
        transform.SetParent(transform.root);
        offset = transform.position - Input.mousePosition;

        image.raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            ToolTipManager.Instance.HideToolTip();
        }

        transform.position = Input.mousePosition + offset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(gameObject.GetComponent<UI_Item>() != null)
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                gameObject.GetComponent<UI_Item>().DropItem();
                return;
            }
        }
        transform.SetParent(_originParent);
        image.raycastTarget = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ToolTipManager.Instance.SetToolTip(item.toolTipText);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            ToolTipManager.Instance.HideToolTip();
        }
    }
}
