using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Vector2 diffVec;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Inventory.inventoryActivated == true)
        {
            diffVec =  new Vector2(eventData.position.x - transform.position.x, eventData.position.y - transform.position.y);
            transform.position = eventData.position - diffVec;
            //transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Inventory.inventoryActivated == true)
        {
            transform.position = eventData.position - diffVec;
            //transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Inventory.inventoryActivated == true)
        {
            transform.position = eventData.position - diffVec;
            //transform.position = eventData.position;
        }

    }
}
