using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] Camera minimapCam;
    [SerializeField] Slider slider;
    [SerializeField] Text sizeText;
    [SerializeField] float minSize;
    [SerializeField] float maxSize;

    float offset;
    private void Start()
    {
        offset = (maxSize - minSize) / 10.0f;
    }

    public delegate void OnValueChanged(float value);
    OnValueChanged OnValueChangedEvent;

    // Mathf.Round : �ݿø�. (float�� ��ȯ)
    // Mathf.Ceil  : �ø�.
    // Mathf.Floor : ����.

    // Mathf.RoundToInt : �ݿø� �Ŀ� int�� ��ȯ.
    bool isButton;      // ��ư�� �����°�?

    public void OnChangedSlider(float value)
    {
        if (isButton == false)
        {
            // value : 0 ~ 10.
            float camSize = minSize + (offset * value);
            OnUpdateMinimap(camSize);
        }

        isButton = false;
    }

    public void OnSizeUp()
    {
        float size = Mathf.Clamp(minimapCam.orthographicSize - offset, minSize, maxSize);
        OnUpdateMinimap(size);
    }
    public void OnSizeDown()
    {
        float size = Mathf.Clamp(minimapCam.orthographicSize + offset, minSize, maxSize);
        OnUpdateMinimap(size);
    }

    private void OnUpdateMinimap(float size)
    {
        isButton = true;
        minimapCam.orthographicSize = size;

        float persent = (size - minSize) / (maxSize - minSize) * 100.0f;
        sizeText.text = string.Format("{0}%", Mathf.Round(persent));

        slider.value = (int)((size - minSize) / offset);
    }
}
