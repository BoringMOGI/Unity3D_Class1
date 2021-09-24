using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InventoryUI;

public class BasicSlotUI : PoolObject<BasicSlotUI>
{
    [SerializeField] protected Image iconImage;

    InventoryUI invenUI;   // �κ��丮 UI �Ŵ���.
    Item hasItem;          // ������ �ִ� ������ ����. 
    AREA_TYPE beforeArea;  // ���� Area ����.

    public virtual void SetSlot(Item item)
    {   
        hasItem = item;
        invenUI = InventoryUI.Instance;

        if (item != null)
        {
            iconImage.sprite = item.itemSprite;
            iconImage.enabled = true;
        }
        else
            iconImage.enabled = false;
    }

    public void OnBeginDrag()
    {
        if (hasItem == null)
            return;

        invenUI.OnBeginDragPreview(hasItem);
        beforeArea = invenUI.GetCurrentArea();
    }
    public void OnDragging()
    {
        if (hasItem == null)
            return;

        invenUI.OnDraggingPreview();
    }
    public void OnEndDrag()
    {
        if (hasItem == null)
            return;

        invenUI.OnEndDragPreview();

        AREA_TYPE currentArea = invenUI.GetCurrentArea();

        if (currentArea == AREA_TYPE.None)              // ���� ��ġ�� �ƹ� ������ �ƴ϶��.
            return;

        if (currentArea == beforeArea)                  // ���� ������ ���� ������ ���� ���.
            return;


        int itemIndex = transform.GetSiblingIndex();    // ���� ���° �ڽ�����?

        switch(currentArea)
        {
            case AREA_TYPE.GroundUI:                                    // ���� ������ �׶��� UI.
                if (beforeArea == AREA_TYPE.EquipUI)                    // ���â -> ���ٴ�
                {
                    if(PlayerController.Instance.OnEquipToGround(hasItem))
                        SetSlot(null);
                }
                else
                {
                    Inventory.Instance.RemoveItem(itemIndex);
                }
                break;
            case AREA_TYPE.InventoryUI:                                 // ���� ������ �κ��丮 UI.
                if (beforeArea == AREA_TYPE.EquipUI)                    // ���â -> �κ��丮.
                {
                    if (PlayerController.Instance.OnEquipToInven(hasItem))
                        SetSlot(null);
                }
                else
                {
                    GroundItemFinder.Instance.OnGroundToInven(itemIndex);
                }
                break;
            case AREA_TYPE.EquipUI:                                     // ���� ������ ��� UI.

                if(beforeArea == AREA_TYPE.GroundUI)                    
                {
                    GroundItemFinder.Instance.OnGroundToEquip(itemIndex);
                }
                else if(beforeArea == AREA_TYPE.InventoryUI)
                {
                    Inventory.Instance.InvenToEquip(itemIndex);
                }
                
                break;
        }
    }
}
