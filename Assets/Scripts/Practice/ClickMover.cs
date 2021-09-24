using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickMover : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    [SerializeField] UnityEvent<Vector3> OnClickPoint;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            OnTouchScreen();
        }
    }

    void OnTouchScreen()
    {
        // (��ũ�� ��ǥ��)���콺 ��ġ.
        // Camera.ScreenToWorldPoint : ��ũ�� ��ǥ�踦 ���� ��ǥ��� ��ȯ.
        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        // ���� ī�޶� ���ߴ� ���콺 �������� ��ġ -> ���� ��ǥ�� ���� �� ���̷� ��ȯ.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, float.MaxValue, groundMask))
        {
            // ������ Ŭ���ߴ�.
            // hit.point�� �⵹ ���� ��ġ.
            OnClickPoint?.Invoke(hit.point);
        }
    }
}
