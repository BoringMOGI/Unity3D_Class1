using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipItem;

public class EquipSlotListUI : Singletone<EquipSlotListUI>
{
    [System.Serializable]
    public class EquipSlot
    {
        public EquipSlotUI slotUI;
        public EquipItem.EQUIPTYPE type;
    }

    [SerializeField] EquipSlot[] equipSlots;

    EquipSlotUI GetSlotUI(EQUIPTYPE type)
    {
        for(int i = 0; i<equipSlots.Length; i++)
        {
            if (equipSlots[i].type == type)     // i��° List�� type�� ���ٸ�
                return equipSlots[i].slotUI;    // �ش� �������� �����Ѵ�.
        }

        return null;
    }

    public void SetEquipItem(EquipItem equipItem)           // �ش� �����ۿ� �´� ���� �־��.
    {
        for(int i = 0; i< equipSlots.Length; i++)
        {
            if(equipSlots[i].type == equipItem.Type)        // i��° ������ Type�� ������ ���.
            {
                equipSlots[i].slotUI.SetSlot(equipItem);    // �ش� ���Կ� ������ ����.
                break;
            }
        }
    }
}
