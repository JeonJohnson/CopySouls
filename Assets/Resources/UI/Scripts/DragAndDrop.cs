using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 ImagePos;
    Vector2 eventPos;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Inventory.inventoryActivated == true)
        {
            ImagePos = transform.position;
            eventPos = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Inventory.inventoryActivated == true)
        {
            Vector2 moveValue = eventData.position - eventPos;
            transform.position = transform.position + new Vector3 (moveValue.x,moveValue.y,0f);
            eventPos = eventData.position;

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Inventory.inventoryActivated == true)
        {
            
        }

    }
}
