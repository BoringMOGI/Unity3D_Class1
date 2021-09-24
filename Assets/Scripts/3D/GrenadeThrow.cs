using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    [SerializeField] Grenade grenadePrefab;
    [SerializeField] Transform throwPivot;
    [SerializeField] float throwPower;

    Grenade grenade = null;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("��ź ���� �� �غ� ��� ���");
            Debug.Log("���� ȿ����");
            CreateGrenade();
        }
        if(Input.GetMouseButtonUp(0))     // ���콺 ���� Ŭ��.
        {
            Throw();                      // ��ź ���� �� ������.
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(grenade != null)
                grenade.Unlock();
        }

        // ����ź�� pivot�� ��ġ�� ��� ����ٳ�� �Ѵ�.
        if (grenade != null)
        {
            grenade.transform.position = throwPivot.position;
            grenade.transform.rotation = throwPivot.rotation;
        }
    }

    void CreateGrenade()
    {
        // ���� üũ.
        // �� ���� ����ź�� ���ٸ�...

        grenade = Instantiate(grenadePrefab, throwPivot.position, throwPivot.rotation);
        grenade.GetComponent<Rigidbody>().useGravity = false;
    }
    void Throw()
    {
        if (grenade == null)
            return;

        // ������ Ŭ�� ���� �� �������� throwPower�� �� ��ŭ ������.
        grenade.Unlock();
        grenade.GetComponent<Rigidbody>().AddForce(transform.forward * throwPower, ForceMode.Impulse);
        grenade.GetComponent<Rigidbody>().useGravity = true;
        grenade = null;
    }
}
