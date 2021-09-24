using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : Controller
{
    static PlayerController instance;
    public static PlayerController Instance => instance;

    [SerializeField] Animator anim;               // �ִϸ��̼�.
    [SerializeField] Transform aimPivot;          // ���ӽ� ��ġ.
    [SerializeField] Inventory inventory;         // �κ��丮.
    [SerializeField] Stateable stat;              // ����.

    [Header("Interaction")]
    [SerializeField] KeyCode interactionKey;      // ��ȣ�ۿ� Ű.
    [SerializeField] Transform searchItemPivot;   // ������ Ž�� ������.
    [SerializeField] float searchRadius;          // ������ Ž�� ����.
    [SerializeField] LayerMask itemMask;          // ������ ����ũ.

    Camera mainCam;
    InventoryUI invenUI;

    // ��� ������.
    Dictionary<EquipItem.EQUIPTYPE, Item> equipList = new Dictionary<EquipItem.EQUIPTYPE, Item>();      

    bool isAim;                     // ���� �����ΰ�.
    bool isAlive;                   // ���� ����.
    float normalFov;                // ���� FOV.
    float aimFov;                   // ���ӽ� FOV.

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mainCam = Camera.main;

        normalFov = mainCam.fieldOfView;
        aimFov = normalFov - 10;
        invenUI = InventoryUI.Instance;

        isAlive = true;

        // dictionary�� EQUIPTYPE�� ���缭 �̸� ����.
        foreach(EquipItem.EQUIPTYPE type in System.Enum.GetValues(typeof(EquipItem.EQUIPTYPE)))
        {
            equipList.Add(type, null);
        }
    }

    private void Update()
    {
        if (isAlive)
        {
            WeaponControl();

            if(invenUI != null && invenUI.IsOpenInventory == false) 
                UpdateSearchItem();     // �κ��丮�� �������� ���� ��.
        }
    }

    // ==================== weapon ===================
    void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            mainGun.Reload();
        }
    }
    void Aim()
    {
        // ����.
        isAim = Input.GetMouseButton(1);
        anim.SetBool("isAim", isAim);

        // ī�޶��� ��ġ.
        Vector3 camPos = isAim ? aimPivot.position : eyePivot.position;
        float fov = isAim ? aimFov : normalFov;

        mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, camPos, 15f * Time.deltaTime);
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, fov, 10f * Time.deltaTime);
    }
    void WeaponControl()
    {
#if UNITY_STANDALONE
        if (InventoryUI.Instance != null && InventoryUI.Instance.IsOpenInventory)       // �κ��丮�� ���������� ����.
            return;

        Reload();
        Aim();

        // �߻�.
        if (Input.GetMouseButton(0))
            Fire();
#endif
    }

    public void OnJoystickFire()
    {
        Fire();
    }

    void UpdateSearchItem()
    {
        ItemObject itemObject = GroundItemFinder.Instance.FirstGroundItem;   // hit�� ������Ʈ�� ItemObject�ΰ�?
        if (itemObject != null)
        {
            // FŰ ������ �Դ´�.
            InteractionUI.Instance.SetInteractionUI(interactionKey, itemObject.HasItem);
            if (Input.GetKeyDown(interactionKey) == false)
                return;

            Item putItem = itemObject.HasItem;           // ������ ������Ʈ ������ ������ ����.

            if (putItem.itemType == Item.ITEMTYPE.Equipment)
            {
                GroundItemFinder.Instance.OnGroundToEquip(-1);  // �ش� �������� ����ض�.
            }
            else
            {
                GroundItemFinder.Instance.OnGroundToInven(-1);  // �ش� �������� �κ��丮�� �־��.
            }
        }
        else
        {
            InteractionUI.Instance.CloseUI();
        }
    }
    

    public Item OnEquipItem(Item item)
    {
        if (item.itemType != Item.ITEMTYPE.Equipment)
            return item;

        EquipItem equipItem = item.ConvertToEquip();        // ����Ϸ��� Item�� EquipItem���� ����Ʈ.
        if (equipItem == null)
            return item;

        Item beforeItem = equipList[equipItem.Type];        // ������ ���� ��� beforeItem���� ����. (null�� �� �ִ�.)
        equipList[equipItem.Type] = equipItem;
        
        EquipSlotListUI.Instance.SetEquipItem(equipItem);   // UI�� ����ϴ� ������ ����.

        return beforeItem;
    }
    public bool OnEquipToInven(Item item)
    {
        EquipItem equip = item.ConvertToEquip();
        if (equip == null)
            return false;

        Item before = equipList[equip.Type];                   // ��� �������� ������.
        inventory.PutItem(before);                             // �κ��丮�� �ִ´�.
        equipList[equip.Type] = null;
        return true;
    }
    public bool OnEquipToGround(Item item)
    {
        EquipItem equip = item.ConvertToEquip();
        if (equip == null)
            return false;

        Item before = equipList[equip.Type];                                // ��� �������� ������.
        equipList[equip.Type] = null;                                       // ������ ��� �迭���� �����Ѵ�.

        Vector3 itemPos = transform.position + (transform.forward * 2f);    // ���� ���� 2���� ��.
        ItemManager.Instance.ConvertToObject(before, itemPos);              // ���ٴڿ� ������.
        
        return true;
    }
    public void OnDamaged(float damage)
    {
        // �´� ��� ���ϱ�.
        StateInfoUI.Instance.SetHp(stat.hp, stat.MaxHp);
    }
    public void OnDead()
    {
        Debug.Log("�÷��̾� ���..");
        isAlive = false;
    }

    private void OnDrawGizmosSelected()
    {
        if(searchItemPivot != null)
            Gizmos.DrawWireSphere(searchItemPivot.position, searchRadius);
    }

    public Vector3 RemoveItemPosition()
    {
        return transform.position + (transform.forward * 2f);
    }
}
