using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singletone<Inventory>
{
    List<Item> itemList = new List<Item>();   // ������ ����Ʈ.
    float currentWeight;                      // ������ ����.
    float maxWeight;                          // ������ �ִ� ����.    

    public event System.Action OnUpdateInventory;  // �κ��丮�� ��ȭ�� ����.
    PlayerController player;

    public int IndexCount => itemList.Count;       // �������� ����.
    public float Weight => currentWeight;
    public float MaxWeight => maxWeight;

    private void Start()
    {
        maxWeight = 100f;
        player = PlayerController.Instance;

        UpdatedInventory();
    }

    public void PutItem(Item putItem)
    {
        Item newItem = putItem.GetCopy();

        // ���� ���԰� ����ϴ� ��ŭ
        int maxAllowedCount = (int)((maxWeight - currentWeight) / putItem.itemWeight);

        // �������� ������ ���ġ ���̶��.
        if (putItem.itemCount <= maxAllowedCount)
        {
            putItem.itemCount = 0;
        }
        else
        {
            newItem.itemCount = maxAllowedCount;
            putItem.itemCount -= maxAllowedCount;
        }

        // ������ ������ �������� �ִٸ� �����Ѵ�.
        foreach (Item item in itemList)
        {
            if ((item != newItem) || item.IsFull)
                continue;

            // ���� �������� �����۰� ���� �������� �����Ѵ�.
            // �ش� �������� ���� ���� ������ �ִ�.
            int totalCount   = item.itemCount + newItem.itemCount;
            int newItemCount = Mathf.Clamp(totalCount - item.maxItemCount, 0, item.maxItemCount);
            int itemCount    = totalCount - newItemCount;

            item.itemCount = itemCount;
            newItem.itemCount = newItemCount;

            if (newItem.itemCount <= 0)
                continue;
        }

        if(newItem.itemCount > 0)
            itemList.Add(newItem);
               
        UpdatedInventory();
    }
    public void RemoveItem(int index)
    {
        if (index < 0 || itemList.Count <= index)
        {
            Debug.Log(string.Format("[RemoveItem] {0}��°�� �κ��丮�� ������ ����ϴ�.", index));
            return;
        }

        // ����Ʈ���� ����.
        Item removedItem = itemList[index];              // ������ ������ ���� ����.
        itemList.RemoveAt(index);                        // ����Ʈ���� ������ ����.

        // ������ ����.
        ItemManager.Instance.ConvertToObject(removedItem, PlayerController.Instance.RemoveItemPosition());

        UpdatedInventory();
    }

    
    public void InvenToEquip(int index)                 // �κ��丮���� ��� �������� �̵�.
    {
        Item requestItem = itemList[index];
        if (requestItem.itemType != Item.ITEMTYPE.Equipment)
            return;

        itemList.RemoveAt(index);
        requestItem = player.OnEquipItem(requestItem);  // ��� ���� �õ�(���X, �̹� ����� �� �־ return�ǰų�)

        if(requestItem != null)
            itemList.Add(requestItem);

        UpdatedInventory();
    }

    public bool IsEnougthItem(AmmoItem.AMMOTYPE ammoType, int count)
    {
        foreach(Item item in itemList)
        {
            if (item.EqualsItem(ammoType) && item.itemCount >= count)
                return true;
        }

        return false;
    }
    public int GetItem(AmmoItem.AMMOTYPE ammoType, int needCount)
    {
        int getCount = 0;                       // �������� ����.

        // needCount��ŭ ��û������ needCount���� ���� �� �� �ִ�.
        for(int i = 0; i<itemList.Count;)
        {
            if (needCount <= 0)
                break;

            Item item = itemList[i];
            if (item.EqualsItem(ammoType) == false)
            {
                i++;
                continue;
            }

            int itemCount = item.itemCount;
            if(itemCount <= needCount)              // (�����)�ش� �������� ������ ���ϴ� �������� ����.
            {
                getCount += itemCount;              // �ش� �������� ��� ������ �߰��Ѵ�.
                needCount -= itemCount;             // �ʿ� ī��Ʈ�� ������ ������ŭ ����.
                itemList.RemoveAt(i);               // i��° �������� ������ 0���� �Ǿ� ����Ʈ���� ����.
            }
            else                                    // �������� ������ ���ϴ� ���� ��ŭ ����ϴ�.
            {
                itemList[i].itemCount -= needCount; // �������� ������ ���ϴ� ���� ��ŭ ����.
                getCount += needCount;              // ���ϴ� ������ŭ getCount�� �߰�.
                needCount = 0;                      // ���ϴ� ������ 0.
                i++;
            }
        }

        UpdatedInventory();

        return getCount;
    }
    public int ItemCount(AmmoItem.AMMOTYPE ammoType)
    {
        int itemCount = 0;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].EqualsItem(ammoType))
                itemCount += itemList[i].itemCount;
        }

        return itemCount;
    }

    private void UpdatedInventory()
    {
        // ���� ���.
        currentWeight = 0f;
        for (int i = 0; i < itemList.Count; i++)
            currentWeight += itemList[i].itemWeight * itemList[i].itemCount;

        if (InventoryUI.Instance == null)
            return;

        InventoryUI.Instance.SetInventory(itemList, currentWeight / maxWeight);     // UI ����.
        OnUpdateInventory?.Invoke();
    }
}
