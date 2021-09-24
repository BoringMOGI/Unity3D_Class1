using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{    
    Transform[] itemPivots;

    // Start is called before the first frame update
    void Start()
    {
        // �� �ڽ�(= ��ġ ����)�� ����.
        itemPivots = new Transform[transform.childCount];
        for (int i = 0; i < itemPivots.Length; i++)
            itemPivots[i] = transform.GetChild(i);


        // �� itemPivots��ŭ �������� �����Ѵ�.
        foreach(Transform pivot in itemPivots)
        {
            // 50% Ȯ���� �ش� pivot���� �������� �ʴ´�.
            if (Random.value < 0.5f)
                continue;

            Spawn(pivot);
        }
    }

    void Spawn(Transform pivot)
    {
        ItemObject newItem = ItemManager.Instance.GetRandomItemObject();
        newItem.transform.position = pivot.transform.position;
    }
}
