using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Joystick : MonoBehaviour
{
    [SerializeField] RectTransform joystick;
    [SerializeField] protected UnityEvent<Vector2> OnMoveStick;
    

    Vector2 originPosition;         // ��ƽ�� �ʱ� ��ġ.
    float stickRadius;              // ��ƽ�� ������.
    bool isModify;                  // ���� ����.

    private void OnEnable()
    {
#if UNITY_STANDALONE
    gameObject.SetActive(false);
#else
        originPosition = joystick.position;
        stickRadius = GetComponent<RectTransform>().sizeDelta.x / 2f;   // ��ƽ�� ��� �ʺ� / 2f.
#endif
    }

    public void OnBeginModify()
    {
        isModify = true;
    }
    public void OnEndModify()
    {
        isModify = false;
        joystick.position = originPosition;     // ��ƽ�� ���� ��ġ�� �ǵ���.
    }

    void Update()
    {
        if (!isModify)
            return;

        // Vector2.normalized : (0.0~1.0)������ ����ȭ �� ���� �����ϴ� ������Ƽ.
        // Vector.Distance(V, V) : �� ���� ������ �Ÿ� ���� flaot�� ����.
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 direction = (mousePosition - originPosition).normalized;
        float distance = Vector2.Distance(originPosition, mousePosition);
        distance = Mathf.Clamp(distance, 0f, stickRadius);      // �Ÿ��� ���� 0~������.

        // ���� * �Ÿ� = �̵���.
        joystick.position = originPosition + (direction * distance);

        float stickPower = distance / stickRadius;              // ��ƽ�� �󸶳� �������°�?(0.0~1.0);
        OnMoveStick?.Invoke(direction * stickPower);            // �������� ������� ����.
        //Debug.Log($"distance:{distance}, vector:{direction}");
    }
}
