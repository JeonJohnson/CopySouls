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

        go_CountImage.SetActive(true);
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

    //���� �ʱ�ȭ
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        item_Image.sprite = null;
        SetColor(0);
        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        //��Ŭ����
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (item.itemType == Enums.ItemType.weapon_Equiptment_Item
                || item.itemType == Enums.ItemType.Defence_Equiptment_Item)
                {
                    //Equipt(rightMouse)
                    Inventory.Instance.Equipt();
                }
                else
                {
                    //Division(shift + rightMouse)
                    if(Input.GetKey(KeyCode.LeftShift))
                    {
                        if (itemCount < 2) return;
                        else
                        {
                            Inventory.Instance.curSlot = this;
                            Inventory.Instance.TryOpenDivision();
                        }
                    }
                    else
                    {
                        if (item.itemType == Enums.ItemType.supply_Item)
                        {
                            //use(rightMouse)
                            Inventory.Instance.curSlot = this;
                            Inventory.Instance.UseSupply();
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
            if (item != null)
            {
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
                if (eventData.pointerCurrentRaycast.gameObject != null)
                {
                    DragSlot.instance.SetColor(0);
                    DragSlot.instance.dragSlot = null;
                }
                else
                {
                    ItemThrow(item,itemCount);
                    DragSlot.instance.SetColor(0);
                    DragSlot.instance.dragSlot = null;
                    ClearSlot();
                }
            }
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
                    if (DragSlot.instance.dragSlot.item.objName == item.objName 
                        && DragSlot.instance.dragSlot.item.itemType == Enums.ItemType.Production_Item)
                    {
                        SumSlot();
                    }
                    else if (DragSlot.instance.dragSlot.item.objName != item.objName)
                    {
                        //�巡�׷� ���
                        if (this.GetComponent<EquiptSlot>() == null)
                        {
                            ChangeSlot();
                        }
                        else
                        {
                            //EquiptChangSlot();
                        }
                    }
                }
                else
                {
                    if (this.GetComponent<EquiptSlot>() == null)
                    {
                        ChangeSlot();
                    }
                    else
                    {
                        //EquiptChangSlot();
                    }
                }
            }
        }
    }
    private void ChangeSlot()
    {
        Item tempItem = item;
        int tempItemCount = itemCount;

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
        int sum = itemCount + DragSlot.instance.dragSlot.itemCount;
        if (sum <= MaxCount && sum >= 1)
        {
            SetSlotCount(DragSlot.instance.dragSlot.itemCount);
            DragSlot.instance.dragSlot.ClearSlot();
        }
        else if(itemCount == MaxCount) return;
        else
        {
            int rest = itemCount + DragSlot.instance.dragSlot.itemCount - MaxCount;
            SetSlotCount(MaxCount - itemCount);
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
