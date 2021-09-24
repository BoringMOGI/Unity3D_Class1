using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponGun : MonoBehaviour
{
    [Header("Bulelt")]
    [SerializeField] AmmoItem.AMMOTYPE ammoType;    // ź�� ����.
    [SerializeField] Bullet bulletPrefab;           // ź�� ������.
    [SerializeField] Transform muzzle;              // �ѱ� ��ġ.
    [SerializeField] float bulletSpeed;             // ź�� �ӵ�.

    [Header("Gun")]
    [SerializeField] Animator anim;                 // �ִϸ��̼�.
    [SerializeField] AudioSource fireAudio;         // �߻� ȿ����.
    [SerializeField] float fireRate;                // ���� �ӵ�.
    [SerializeField] int maxAmmoCount;              // �� ���������� �Ѿ�.
    [SerializeField] Vector2 gunRecoil;             // �ѱ� �ݵ�.
    [SerializeField] float gunPower;                // �ѱ� ������.

    public UnityEvent<float, float> OnAddRecoil;    // �ѱ� �ݵ� �̺�Ʈ.
    public UnityEvent<int, int> OnUpdateAmmoUI;     // ź���� ������ �׸��� UI ����.

    int ammoCount;                                  // ���� �Ѿ��� ����.
    float nextFireTime;                             // �߻� ���� �ð�.

    bool isReloading;                               // ���� ���ΰ�?
    Inventory inven;                                // �κ��丮.

    private int AmmoCount
    {
        get
        {
            return ammoCount;
        }
        set
        {
            // ź���� ���� ���� �Ǿ���.
            ammoCount = Mathf.Clamp(value, 0, maxAmmoCount);
            OnUpdateAmmoUI?.Invoke(ammoCount, inven.ItemCount(ammoType));
        }
    }

    private void Start()
    {
        inven = Inventory.Instance;
        inven.OnUpdateInventory += OnUpdatedInven;

        AmmoCount = 0;
     
    }
    private void OnUpdatedInven()
    {
        // �κ��丮�� �����۵��� ��ȭ�ϸ� UI�� �����Ѵ�.
        OnUpdateAmmoUI?.Invoke(ammoCount, inven.ItemCount(ammoType));
    }

    public void Fire(Vector3 hitPoint)
    {
        if(nextFireTime <= Time.time && ammoCount > 0 && isReloading == false)
        {
            AmmoCount -= 1;

            anim.SetTrigger("onFire");
            fireAudio.Play();
            nextFireTime = Time.time + fireRate;
            Bullet newBullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.LookRotation(hitPoint - muzzle.position));
            newBullet.Shoot(bulletSpeed, gunPower);

            // �ݵ�.
            OnAddRecoil?.Invoke(Random.Range(-gunRecoil.x, gunRecoil.x), gunRecoil.y);
            //MouseLook.Instance.AddRecoil(Random.Range(-gunRecoil.x, gunRecoil.x), gunRecoil.y);
        }
    }

    public void Reload()
    {
        // ���ε� ���� �ƴ� ���.
        // ���� ź���� �ִ� ź�ຸ�� ���� ���.
        // �κ��丮�� ź���� 1���̶� ���� ���.
        if (isReloading == false && ammoCount < maxAmmoCount && inven.IsEnougthItem(ammoType, 1))
        {
            isReloading = true;
            anim.SetTrigger("onReload");
        }
    }
    public void OnEndReload()
    {
        isReloading = false;

        int needCount = maxAmmoCount - ammoCount;           // �ʿ� ź��.
        int getItem = inven.GetItem(ammoType, needCount);   // ���� ź��.

        AmmoCount += getItem;
    }

}
