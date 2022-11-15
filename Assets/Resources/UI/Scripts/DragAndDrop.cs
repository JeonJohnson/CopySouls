using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Inventory.inventoryActivated == true && Inventory.SortActivated == false)
        {
            transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Inventory.inventoryActivated == true && Inventory.SortActivated == false)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Inventory.inventoryActivated == true && Inventory.SortActivated == false)
        {
            transform.position = eventData.position;
        }

    }
}
