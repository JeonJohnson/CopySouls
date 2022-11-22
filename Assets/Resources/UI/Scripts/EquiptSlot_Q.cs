using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquiptSlot_Q : EquiptSlot
{
    void Start()
    {
        
    }
    public void matchEquiptmentSlot_Q(Item _item)
    {
        Item_Image.sprite = _item.itemImage;
        //»ö, activated ¿¬°á
    }

}
