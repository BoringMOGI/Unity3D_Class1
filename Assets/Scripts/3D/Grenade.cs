using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] float delay;               // �� �� �ڿ� �����°�?
    [SerializeField] float explodeRadius;       // ���� ����.
    [SerializeField] ParticleSystem effect;     // ���� ����Ʈ.

    float countDown = 0.0f;
    bool isLock = true;                         // ���ʿ��� ����ִ�.

    void Start()
    {
        isLock = true;
    }


    public void Unlock()
    {
        if (isLock == false)
            return;

        Debug.Log("Grenade Unlock!");
        isLock = false;
        countDown = Time.time + delay;          // ���� ������ �ð� + dealy.
    }


    private void Update()
    {
        if (isLock)                             // lock���¿����� Update�� �������� �ʴ´�.
            return;

        if(countDown <= Time.time)              // ������ �ð��� countDown�� �Ѿ��� ���.
        {
            Debug.Log("Explode!!");
            Instantiate(effect, transform.position, transform.rotation);        // ���� ����Ʈ ����.
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRadius);
    }

}
