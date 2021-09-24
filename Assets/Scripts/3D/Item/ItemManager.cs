using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data;

public class ItemManager : Singletone<ItemManager>
{
    [SerializeField] ItemObject itemObjectPrefab;

    List<Item> itemList;
    RandomBox<Item> randomBox;

    new void Awake()
    {
        base.Awake();
        ReadCsvFile();
    }

    [ContextMenu("Read CSV File")]
    public void ReadCsvFile()
    {
        itemList = new List<Item>();

        List<Dictionary<string, object>> data = CSVReader.Read("ItemData");
        for (int i = 0; i < data.Count; i++)
            CreateItem(data[i]);

        randomBox = new RandomBox<Item>();
        for(int i = 0; i<itemList.Count; i++)
            randomBox.Push(itemList[i], 10);
    }

    private void CreateItem(Dictionary<string, object> data)
    {        
        Item item = null;

        Item.ITEMTYPE itemType = (Item.ITEMTYPE)System.Enum.Parse(typeof(Item.ITEMTYPE), data["ItemType"].ToString());
        switch (itemType)
        {
            case Item.ITEMTYPE.Ammo:
                item = new AmmoItem(data);                
                break;

            case Item.ITEMTYPE.Equipment:
                item = new EquipItem(data);
                break;

            case Item.ITEMTYPE.Weapon:
                // item = new WeaponItem(data);
                break;
        }

        itemList.Add(item);
    }

    public Item GetItem(AmmoItem.AMMOTYPE ammoType)
    {
        for(int i = 0; i<itemList.Count; i++)
        {
            if (itemList[i].EqualsItem(ammoType))
                return itemList[i].GetCopy();
        }

        return null;
    }
    public Item GetItem(EquipItem.EQUIPTYPE equipType)
    {
        return null;
    }
    public Item GetRandomItem()
    {
        return randomBox.Pick().GetCopy();
    }
    public ItemObject GetRandomItemObject()
    {
        Item randomItem = GetRandomItem();                            // ������ ������ ������ ����.
        return ConvertToObject(randomItem);                           // �ش� �������� ������Ʈ�� ��ȯ.
    }
    public ItemObject ConvertToObject(Item item)
    {
        ItemObject newItemObject = Instantiate(itemObjectPrefab);     // �ش� �������� ������ �ִ� ������Ʈ ����.
        newItemObject.Setup(item);                                    // ������ ������Ʈ ����.

        return newItemObject;                                         // ����.
    }
    public ItemObject ConvertToObject(Item item, Vector3 position)
    {
        ItemObject newItemObject = ConvertToObject(item);
        newItemObject.transform.position = position;

        return newItemObject;
    }
}
