using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stateable : MonoBehaviour
{
    [SerializeField] float maxHp;               // ü��.
    [SerializeField] float maxStamina;          // ���׹̳�.
    [SerializeField] float maxBreath;           // ȣ��.

    [HideInInspector] public float hp;
    [HideInInspector] public float stamina;
    [HideInInspector] public float breath;

    public float MaxHp => maxHp;
    public float MaxStamina => maxStamina;
    public float MaxBreath => maxBreath;
    

    void Start()
    {
        hp = maxHp;
        stamina = maxStamina;
        breath = maxBreath;
    }
}
