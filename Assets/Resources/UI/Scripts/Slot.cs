using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//���콺 �̺�Ʈ ��� ���
using UnityEngine.EventSystems;

//RayChast üũ ����(���� �����ִ� �̹����� �����ɽ�Ʈ�� �����ϴϱ�)

//IPointerClickHandler : Ŭ������ڵ鷯(�������̽�)
//IBeginDragHandler : �巡�� ���� �̺�Ʈ
//IDragHandler : �巡�� �� �̺�Ʈ
//IEndDragHandler : �巡�� �� �̺�Ʈ
//IDropHandler : ���콺 �� �̺�Ʈ
public class Slot : MonoBehaviour, IPointerClickHandler , IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //��ĭ�� �� �� �ִ� �ִ� ������ ����
    public int MaxCount = 10;
    //ȹ���� ������
    public Item item;
    //ȹ���� ������ ����
    public int itemCount;
    //������ �̹���
    public Image item_Image;
    //������ ���� �ؽ�Ʈ
    [SerializeField]
    protected Text text_Count;
    //������ ���� �̹���(�������� �������� ���)
    [SerializeField]
    private GameObject go_CountImage;
    [SerializeField]
    private Text Register_Text;

    public bool isQuick;

    void Start()
    {
    }

    //������ ��������Ʈ ���İ� ����
    private void SetColor(float _alpha)
    {
        Color color = item_Image.color;
        color.a = _alpha;
        item_Image.color = color;
    }

    //������ ȹ��
    public void AddItem(Item _item,int _count = 1)
    {
        item = _item;
        itemCount = _count;
        item_Image.sprite = item.itemImage;

        if(go_CountImage != null) go_CountImage.SetActive(true);

        text_Count.text = itemCount.ToString();

        SetColor(1);
    }

    //������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();
        if (itemCount <= 0) ClearSlot();
    }

    public void SetRegister(bool value)
    {
        isQuick = value;
        Register_Text.gameObject.SetActive(value);
    }

    //���� �ʱ�ȭ
    private void ClearSlot()
    {
        isQuick = false;
        item = null;
        itemCount = 0;
        item_Image.sprite = null;
        SetColor(0);
        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Inventory.SelectionActivated)
        {
            if (eventData.pointerCurrentRaycast.gameObject != Inventory.Instance.SelectParent)
            {
                Inventory.Instance.CloseSelection();
                Inventory.Instance.curSlot = null;
            }
        }

        //��Ŭ����
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                Inventory.Instance.curSlot = this;

                if (item.itemType == Enums.ItemType.weapon_Equiptment_Item
                || item.itemType == Enums.ItemType.Defence_Equiptment_Item)
                {
                    //Equipt(rightMouse)
                    //���� â ����(���, �����)
                    Inventory.Instance.TryOpenSelection(item.itemType, eventData.position);
                }
                else
                {
                    //Division(shift + rightMouse)
                    if(Input.GetKey(KeyCode.LeftShift))
                    {
                        if (itemCount < 2) return;
                        else
                        {
                            Inventory.Instance.TryOpenDivision();
                        }
                    }
                    else
                    {
                        if (item.itemType == Enums.ItemType.supply_Item || item.itemType == Enums.ItemType.Production_Item)
                        {
                            //use(rightMouse)
                            Inventory.Instance.TryOpenSelection(item.itemType, eventData.position);
                        }
                        else return; 
                    }

                }
            }
        }
    }


    //�巡�� ó�� ���۽� ������ġ�� ���콺 ��ġ�� �ޱ�
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                if (Inventory.DivisionActivated) return;

                DragSlot.instance.dragSlot = this;
                DragSlot.instance.DragSetImage(item);
                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    //�巡�� ���϶� ������ ���콺 ��ġ ����ٴϰ� �ϱ�
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Inventory.DivisionActivated) return;

            if (item != null)
            {
                Inventory.Instance.curSlot = this;

                DragSlot.instance.transform.position = eventData.position;
            }
        }
    }

    //�巡�װ� ������ �� ������ �ٽ� ���� ��ġ�� ���ư����ϱ�
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (DragSlot.instance.dragSlot != null)
            {
                GameObject obj = eventData.pointerCurrentRaycast.gameObject;
                if (obj != null)
                {
                    if(obj.GetComponentInParent<QuickSlot>() != null)
                    {
                        QuickSlot quickSlot = obj.GetComponentInParent<QuickSlot>();
                        quickSlot.DragRegister(Inventory.Instance.curSlot,DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
                        DragSlot.instance.SetColor(0);
                        DragSlot.instance.dragSlot = null;
                    }
                    else
                    {
                        DragSlot.instance.SetColor(0);
                        DragSlot.instance.dragSlot = null;
                    }
                }
                else
                {
                    ItemThrow(item,itemCount);
                    DragSlot.instance.SetColor(0);
                    DragSlot.instance.dragSlot = null;
                    ClearSlot();
                }
            }
            Inventory.Instance.curSlot = null;
        }
    }

    //EndDrag�� OnDrop�� ����
    //EndDrag�� �ƹ������� �巡�� ����� ȣ��
    //OnDrop�� �ش� ������ ������ ����� ȣ��
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (DragSlot.instance.dragSlot != null)
            {
                if (item != null)
                {
                    if (DragSlot.instance.dragSlot.isQuick && !isQuick)
                    {
                        ChangeSlot();
                    }
                    else if (!DragSlot.instance.dragSlot.isQuick && isQuick)
                    {
                        if (DragSlot.instance.dragSlot.item.objName == item.objName
                            && (DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.Production_Item
                            || DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.supply_Item))
                        {
                            SumSlot();
                        }
                        else ChangeSlot();
                    }
                    else if (!DragSlot.instance.dragSlot.isQuick && !isQuick)
                    {
                        if (DragSlot.instance.dragSlot.item.objName == item.objName
                            && (DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.Production_Item
                            || DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.supply_Item))
                        {
                            SumSlot();
                        }
                        else ChangeSlot();
                    }
                    else if (DragSlot.instance.dragSlot.isQuick && isQuick)
                    {
                        ChangeSlot();
                    }
                }
                else ChangeSlot();
            }
        }
    }
    private void ChangeSlot()
    {
        if (Inventory.DivisionActivated) return;

        Item tempItem = item;
        int tempItemCount = itemCount;
        if (DragSlot.instance.dragSlot.isQuick)
        {
            if(!isQuick)
            {
                UiManager.Instance.quickSlot1.invenSlot = this;
                SetRegister(true);
                DragSlot.instance.dragSlot.SetRegister(false);
            }
        }
        else
        {
            if(isQuick)
            {
                UiManager.Instance.quickSlot1.invenSlot = DragSlot.instance.dragSlot;
                SetRegister(false);
                DragSlot.instance.dragSlot.SetRegister(true);
            }
        } 

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);
        if(tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }

    }
    private void SumSlot()
    {
        if (Inventory.DivisionActivated) return;

        int sum = itemCount + DragSlot.instance.dragSlot.itemCount;

        if (sum <= MaxCount && sum >= 1)
        {
            SetSlotCount(DragSlot.instance.dragSlot.itemCount);
            if (isQuick)
            {
                if(DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.Production_Item)
                {
                    UiManager.Instance.quickSlot1.SetSlotCount_q(DragSlot.instance.dragSlot.itemCount);
                }
                else if (DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.supply_Item)
                {
                    UiManager.Instance.quickSlot3.SetSlotCount_q(DragSlot.instance.dragSlot.itemCount);
                }
            }
            DragSlot.instance.dragSlot.ClearSlot();
        }
        else if(itemCount == MaxCount) return;
        else
        {
            int rest = itemCount + DragSlot.instance.dragSlot.itemCount - MaxCount;
            SetSlotCount(MaxCount - itemCount);
            if (isQuick)
            {
                if (DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.Production_Item)
                {
                    UiManager.Instance.quickSlot1.SetSlotCount_q(MaxCount - UiManager.Instance.quickSlot1.itemCount);
                }
                else if (DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.supply_Item)
                {
                    UiManager.Instance.quickSlot3.SetSlotCount_q(MaxCount - UiManager.Instance.quickSlot3.itemCount);
                }
            }
            DragSlot.instance.dragSlot.ClearSlot();
            Inventory.Instance.DivisionItemIn(item, rest);
        }
    }
    private void ItemThrow(Item _item, int _count)
    {
        Debug.Log(item.objName + " " + itemCount + " �� ������");
        DragSlot.instance.dragSlot.ClearSlot();
    }

}

    //    if (DragSlot.instance.dragSlot.item.objName == item.objName
    //        && (DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.Production_Item
    //        || DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.supply_Item))
    //    {
    //        SumSlot();
    //    }
    //    //else if(DragSlot.instance.dragSlot != this)
    //    else if (DragSlot.instance.dragSlot.item.objName != item.objName)
    //    {
    //        ChangeSlot();
    //    }
    //}
    //else
    //{
    //    ChangeSlot();
    //}