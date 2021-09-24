using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutManager : Singletone<ShortcutManager>
{
    public delegate void ShortcutEvent(KeyCode key);         // ��������Ʈ ����.

    event ShortcutEvent OnShutcut;                           // �̺�Ʈ �Լ� ����.

    public void RegestedShutcut(ShortcutEvent OnShutcut)
    {
        this.OnShutcut += OnShutcut;
    }
    public void RemoveShortcut(ShortcutEvent OnShutcut)
    {
        this.OnShutcut -= OnShutcut;
    }

    private void Update()
    {
        foreach(KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if(Input.GetKeyDown(key))
                OnShutcut?.Invoke(key);
        }
    }
}
