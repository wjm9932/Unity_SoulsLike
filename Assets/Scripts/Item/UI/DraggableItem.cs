using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
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

    private void Awake()
    {
        image = GetComponent<Image>(); 
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
        transform.position = Input.mousePosition + offset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_originParent);
        image.raycastTarget = true;
    }
}
