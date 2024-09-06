using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UI_InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            DraggableItem dropped = eventData.pointerDrag.gameObject.GetComponent<DraggableItem>();
            dropped.originParent = transform;
        }
    }
}
