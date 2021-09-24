using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(OnStart());
    }

    IEnumerator OnStart()
    {
        bool[] beforeActive = new bool[transform.childCount];       // �ڽĵ��� ���� ���� ����.
        for(int i = 0; i<transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;    // i��° �ڽ� ������Ʈ.
            beforeActive[i] = child.activeSelf;                     // ���� ���� ����.
            child.SetActive(true);                                  // �ڽ� ������Ʈ Ȱ��ȭ.
        }
        
        yield return new WaitForEndOfFrame();                       // 1������ ���������� ��ٸ�.

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.SetActive(beforeActive[i]);                       // ���� ���·� ������.
        }
    }

}
