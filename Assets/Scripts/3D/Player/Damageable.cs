using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Stateable))]
public class Damageable : MonoBehaviour
{
    [SerializeField] Stateable stat;

    [Header("Evnets")]
    [SerializeField] UnityEvent<float> OnDamagedEvent;
    [SerializeField] UnityEvent OnDeadEvent;

    private void Start()
    {
        stat = GetComponent<Stateable>();
    }

    public void OnDamaged(float damage)
    {
        stat.hp = Mathf.Clamp(stat.hp - damage, 0, stat.MaxHp);
        OnDamagedEvent?.Invoke(damage);     // OnDamagedEvent�� null�� �ƴ� ��� ȣ���϶�.

        if (stat.hp <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }
}
