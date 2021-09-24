using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSoltListUI : PoolManager<ItemSoltListUI, BasicSlotUI>
{
    [SerializeField] RectTransform uiTransform;         // �� �ڽ��� Rect.
    [SerializeField] Transform itemListParent;          // itemList�� �θ� ������Ʈ.

    public RectTransform Rect => uiTransform;

    public void SetItemList(List<Item> itemList)
    {
        Clear();
        for (int i = 0; i < itemList.Count; i++)
        {
            BasicSlotUI pool = GetPool();
            pool.SetSlot(itemList[i]);
            pool.transform.SetParent(itemListParent);
        }
    }
}
