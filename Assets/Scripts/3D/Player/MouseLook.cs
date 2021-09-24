using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : Singletone<MouseLook>
{
    [SerializeField] Transform playerBody;
    float xRotation = 0f;
    
    Vector2 recoil;         // �ݵ�.
    bool isUpdate;          // ����� �����ϴ°�?

    private void Start()
    {
#if UNITY_STANDALONE
        //Cursor.lockState = CursorLockMode.Locked;       // ���콺 ���α�.
        isUpdate = true;
#else
        isUpdate = false;
#endif
    }

    public void AddRecoil(float x, float y)
    {
        recoil = new Vector2(x, y);
    }

    private void Update()
    {
#if UNITY_STANDALONE
        if (isUpdate == false || (InventoryUI.Instance != null && InventoryUI.Instance.IsOpenInventory))
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        OnMouseLook(new Vector2(mouseX, mouseY));
#endif
    }

    public void OnMouseLook(Vector2 axis)
    {
        // ���� ȸ��.
        axis.x += recoil.x;
        playerBody.Rotate(Vector2.up * axis.x);

        // ���� ȸ��.
        xRotation -= axis.y;
        xRotation -= recoil.y;

        // ���� -60. �ִ� 60
        xRotation = Mathf.Clamp(xRotation, -60.0f, 60.0f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        recoil = Vector2.zero;
    }

    public void OnStopUpdate()
    {
        isUpdate = false;
    }
  
}
