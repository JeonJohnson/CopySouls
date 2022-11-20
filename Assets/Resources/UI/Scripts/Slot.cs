using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//마우스 이벤트 담당 헤더
using UnityEngine.EventSystems;

//RayChast 체크 해제(위에 덥고있는 이미지가 레이케스트를 방해하니까)

//IPointerClickHandler : 클릭담당핸들러(인터페이스)
//IBeginDragHandler : 드래그 시작 이벤트
//IDragHandler : 드래그 중 이벤트
//IEndDragHandler : 드래그 끝 이벤트
//IDropHandler : 마우스 끝 이벤트
public class Slot : MonoBehaviour, IPointerClickHandler , IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    //한칸에 들어갈 수 있는 최대 아이템 개수
    public int MaxCount = 10;
    //획든한 아이템
    public Item item;
    //획득한 아이템 개수
    public int itemCount;
    //아이템 이미지
    public Image item_Image;
    //아이템 개수 텍스트
    [SerializeField]
    protected Text text_Count;
    //아이템 갯수 이미지(아이템이 있을때만 출력)
    [SerializeField]
    private GameObject go_CountImage;
    [SerializeField]
    private Text Register_Text;

    public bool isQuick;

    void Start()
    {
    }

    //아이템 스프라이트 알파값 조절
    private void SetColor(float _alpha)
    {
        Color color = item_Image.color;
        color.a = _alpha;
        item_Image.color = color;
    }

    //아이템 획득
    public void AddItem(Item _item,int _count = 1)
    {
        item = _item;
        itemCount = _count;
        item_Image.sprite = item.itemImage;

        if(go_CountImage != null) go_CountImage.SetActive(true);

        text_Count.text = itemCount.ToString();

        SetColor(1);
    }

    //아이템 개수 조정
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

    //슬롯 초기화
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

        //우클릭시
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                Inventory.Instance.curSlot = this;

                if (item.itemType == Enums.ItemType.weapon_Equiptment_Item
                || item.itemType == Enums.ItemType.Defence_Equiptment_Item)
                {
                    //Equipt(rightMouse)
                    //선택 창 띄우기(장비, 퀵등록)
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


    //드래그 처음 시작시 슬롯위치를 마우스 위치로 받기
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

    //드래그 중일때 슬롯이 마우스 위치 따라다니게 하기
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

    //드래그가 끝났을 떄 슬롯이 다시 원래 위치로 돌아가게하기
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

    //EndDrag와 OnDrop의 차이
    //EndDrag는 아무데서나 드래그 종료시 호출
    //OnDrop은 해당 프리팹 위에서 종료시 호출
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
        Debug.Log(item.objName + " " + itemCount + " 개 버리기");
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