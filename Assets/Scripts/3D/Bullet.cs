using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Transform bulletHolePrefab;

    float power;

    private void OnCollisionEnter(Collision collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.OnDamaged(power);
        }

        Transform bulletHole = Instantiate(bulletHolePrefab, transform.position, Quaternion.LookRotation(collision.contacts[0].normal));

        Destroy(gameObject);
    }

    public void Shoot(float moveSpeed, float power)
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * moveSpeed;         // �ӵ�(S) = ���� ����(V) * �̵� �ӵ�(S)

        this.power = power;
    }
}
