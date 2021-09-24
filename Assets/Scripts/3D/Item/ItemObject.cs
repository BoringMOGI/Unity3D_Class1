using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] Transform spriteObject;

    public Item HasItem;
    Camera eyeCam;

    public void Setup(Item hasItem)
    {
        HasItem = hasItem;

        spriteObject.GetComponent<SpriteRenderer>().sprite = HasItem.itemSprite;

        eyeCam = Camera.main;

        StartCoroutine(Fly());
    }
    IEnumerator Fly()
    {
        Vector3 downPosition = spriteObject.transform.localPosition;        // ���� ��ġ.
        Vector3 upPosition = downPosition + (Vector3.up * 0.2f);            // ���� ��ġ���� ���� 0.5.

        bool isDown = true;
        while(true)
        {
            isDown = !isDown;
            Vector3 destination = isDown ? downPosition : upPosition;

            // �������� �̵�.
            while (spriteObject.localPosition != destination)
            {
                Vector3 pos = Vector3.MoveTowards(spriteObject.localPosition, destination, 0.5f * Time.deltaTime);
                spriteObject.localPosition = pos;

                yield return null;
            }

            // �����ϸ� 0.1�� ���.
            // yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        transform.LookAt(eyeCam.transform.position);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
