using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUI : Singletone<InventoryUI>
{
    public enum AREA_TYPE
    {
        None,
        GroundUI,
        InventoryUI,
        EquipUI,
    }

    [Header("Ground")]
    [SerializeField] ItemSoltListUI groundItemUI;       // �ٴ� ������ ����Ʈ UI.
    [SerializeField] RectTransform groundItemArea;      // �ٴ� ������ UI ����.

    [Header("Inventory")]
    [SerializeField] ItemSoltListUI inventoryItemUI;    // �κ��丮 ����Ʈ UI.
    [SerializeField] RectTransform inventoryUIArea;     // �κ��丮 UI ����.

    [Header("Equip")]
    [SerializeField] EquipSlotListUI qeuipItemUI;
    [SerializeField] RectTransform equipUIArea;

    [Header("Etc")]
    [SerializeField] BasicSlotUI previewItem;            // �̸����� ������.
    [SerializeField] Image weightImage;                 // ���� �̹���.

    public bool IsOpenInventory => gameObject.activeSelf;

    private void Start()                    // ���� ������Ʈ�� ������ ����(1ȸ)
    {
        gameObject.SetActive(false);
        ShortcutManager.Instance.RegestedShutcut(OnSwitchInventory);
    }
    private void OnEnable()                 // ���� ������Ʈ�� ������ ����(�ݺ�)
    {
#if UNITY_STANDALONE
        Cursor.lockState = CursorLockMode.None;
#endif
    }
    private void OnDisable()                // ���� ������Ʈ�� ������ ����(�ݺ�)
    {
#if UNITY_STANDALONE
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }
    private void OnDestroy()
    {
        ShortcutManager.Instance.RemoveShortcut(OnSwitchInventory);
    }

    
    // �κ��丮 ����.
    public void SetInventory(List<Item> itemList, float weightAmount)
    {
        inventoryItemUI.SetItemList(itemList);
        weightImage.fillAmount = weightAmount;
    }
    public void SetGroundItem(List<Item> itemList)
    {
        groundItemUI.SetItemList(itemList);
    }
    public void OnSwitchInventory(KeyCode key)
    {
        if (key != KeyCode.Tab)
            return;

        gameObject.SetActive(!gameObject.activeSelf);
    }

    public AREA_TYPE GetCurrentArea()       // ȣ��Ǵ� ���� ���콺 �����Ͱ� �ִ� ����.
    {
        AREA_TYPE area = AREA_TYPE.None;
        if (RectTransformUtility.RectangleContainsScreenPoint(groundItemArea, Input.mousePosition))
        {
            area = AREA_TYPE.GroundUI;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(inventoryUIArea, Input.mousePosition))
        {
            area = AREA_TYPE.InventoryUI;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(equipUIArea, Input.mousePosition))
        {
            area = AREA_TYPE.EquipUI;
        }

        return area;
    }


    // �̺�Ʈ �Լ�.
    public void OnBeginDragPreview(Item item)
    {
        previewItem.gameObject.SetActive(true);
        previewItem.SetSlot(item);
    }
    public void OnDraggingPreview()
    {
        previewItem.transform.position = Input.mousePosition;
    }
    public void OnEndDragPreview()
    {
        previewItem.gameObject.SetActive(false);
    }
}
