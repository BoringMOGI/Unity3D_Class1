using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItemFinder : Singletone<GroundItemFinder>
{
    List<ItemObject> groundItemList = new List<ItemObject>();       // ��ü �˻�.
    ItemObject groundItemOfForword = null;                              // Raycast �˻�.

    [Header("Eye")]
    [SerializeField] Transform eyePivot;          // ������ Ž�� ������(��)
    [SerializeField] float eyeDistance;           // ������ Ž�� ����(��)

    [Header("Ground")]
    [SerializeField] Transform searchItemPivot;   // ������ Ž�� ������.
    [SerializeField] float searchRadius;          // ������ Ž�� ����.
    [SerializeField] LayerMask itemMask;          // ������ ����ũ.

    public ItemObject FirstGroundItem             // ù��° Ž�� ������.
    {
        get
        {
            if (groundItemOfForword != null)
                return groundItemOfForword;
            else if (groundItemList.Count <= 0)
                return null;
            else
                return groundItemList[0];
        }
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        UpdateGroundItem();
    }

    void UpdateGroundItem()
    {
        // Raycast�� �ϳ��� �˻�.
        RaycastHit rayHit;
        if (Physics.Raycast(eyePivot.position, eyePivot.forward, out rayHit, eyeDistance, itemMask))
            groundItemOfForword = rayHit.collider.GetComponent<ItemObject>();
        else
            groundItemOfForword = null;


        // ShereCast�� ��ü �˻�.
        RaycastHit[] hits = Physics.SphereCastAll(searchItemPivot.position, searchRadius, Vector3.down, 2f, itemMask);
        groundItemList.Clear();

        List<Item> itemList = new List<Item>();

        // �ٴ��� Ž���Ѵ�.
        foreach (RaycastHit hit in hits)
        {
            ItemObject groundItem = hit.collider.GetComponent<ItemObject>();
            if (groundItem != null)
            {
                groundItemList.Add(groundItem);                 // ������ ������Ʈ�� ����Ʈ.
                itemList.Add(groundItem.HasItem);               // �������� ����Ʈ.
            }
        }

        if(InventoryUI.Instance != null)
            InventoryUI.Instance.SetGroundItem(itemList);           // �������� ����Ʈ�� �κ��丮�� ����.
    }

    public void OnGroundToInven(int index)
    {
        ItemObject itemObject = null;
        if (index < 0)
            itemObject = FirstGroundItem;
        else
            itemObject = groundItemList[index];

        Item putItem = itemObject.HasItem;                      // ItemObject�� ������ �ִ� Item.
        Inventory.Instance.PutItem(putItem);                    // �κ��丮�� ����.

        if(putItem.itemCount <= 0)
            Destroy(itemObject.gameObject);
        else
            itemObject.HasItem = putItem;
    }
    public void OnGroundToEquip(int index)
    {
        ItemObject itemObject = null;
        if (index < 0)
            itemObject = FirstGroundItem;
        else
            itemObject = groundItemList[index];

        // �ش� �������� ����ΰ�?
        if (itemObject.HasItem.itemType != Item.ITEMTYPE.Equipment)
            return;

        Vector3 objectPos = itemObject.transform.position;      // ���� ������ ������Ʈ�� ��ġ.
        Item requestItem = itemObject.HasItem;                  // ������ ������Ʈ���� ������ ������ ����.
        Destroy(itemObject.gameObject);                         // ������ ������Ʈ ����.

        requestItem = PlayerController.Instance.OnEquipItem(requestItem);    // ���� �õ�.
        if (requestItem != null)                                             // ��� ��ü.
        {
            ItemManager.Instance.ConvertToObject(requestItem, objectPos);    // ������ ����ϴ� ��� ������ �ٴڿ� ����.
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(eyePivot != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(eyePivot.position, eyePivot.forward * eyeDistance);
        }
    }
}
