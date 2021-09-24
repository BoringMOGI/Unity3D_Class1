using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] Transform target;      // ����ٴ� ���.
    [SerializeField] Vector3 offset;        // ������κ����� ��ġ.
    [SerializeField] bool fixX;             // x���� ���߰ڴ�.
    [SerializeField] bool fixY;             // y���� ���߰ڴ�.
    [SerializeField] bool fixZ;             // z���� ���߰ڴ�.


    // Update() ���Ŀ� ȣ��.
    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        if (fixX)
            pos.x = target.position.x + offset.x;
        if (fixY)
            pos.y = target.position.y + offset.y;
        if (fixZ)
            pos.z = target.position.z + offset.z;

        transform.position = pos;
    }
}
