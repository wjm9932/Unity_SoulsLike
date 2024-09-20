using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UI_InventorySlot : MonoBehaviour, IDropHandler
{
    public enum SlotType
    {
        QuickSlot,
        InventorySlot
    }
    
    [SerializeField]
    SlotType slotType;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            if(slotType == SlotType.QuickSlot)
            {
                if(eventData.pointerDrag.gameObject.GetComponent<UsableItem>() != null)
                {
                    DraggableItem dropped = eventData.pointerDrag.gameObject.GetComponent<DraggableItem>();
                    dropped.originParent = transform;
                }
            }
            else
            {
                DraggableItem dropped = eventData.pointerDrag.gameObject.GetComponent<DraggableItem>();
                dropped.originParent = transform;
            }
        }
    }
}
