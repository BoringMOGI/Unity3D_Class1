using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] RectTransform compassImage;
    [SerializeField] RectTransform leftImage;
    [SerializeField] RectTransform rightImage;

    float targetRotation => target.eulerAngles.y;         // ���� �������� ���� Ÿ���� ȸ�� ��.
    float compassLength => compassImage.sizeDelta.x;      // ��ħ�� �̹����� ����.
    
    private void Start()
    {
        gameObject.SetActive(true);
        ShortcutManager.Instance.RegestedShutcut(OnSwitchCompass);
    }
    private void OnDestroy()
    {
        ShortcutManager.Instance.RemoveShortcut(OnSwitchCompass);
    }

    void Update()
    {
        RotateCompass();
    }

    private void RotateCompass()
    {
        float positionX = (targetRotation / 360.0f) * compassLength * -1f;

        compassImage.localPosition = new Vector3(positionX, 0f, 0f);                 // ��ħ���� X�� ��ġ.
        leftImage.localPosition = compassImage.localPosition;                        // ���� ���� �̹����� X�� ��ġ.
        rightImage.localPosition = new Vector3(positionX + compassLength, 0f, 0f);   // ������ ���� �̹����� X�� ��ġ.
    }

    public void OnSwitchCompass(KeyCode key)
    {
        if (key != KeyCode.Tab)
            return;

        gameObject.SetActive(!gameObject.activeSelf);
    }
}
