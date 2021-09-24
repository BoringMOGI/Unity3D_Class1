using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerQ : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;    // �׺���̼� ����.
    [SerializeField] float moveSpeed;       // �̵� �ӵ�.

    Vector3 destination;                    // ������.

    private void Start()
    {
        destination = transform.position;
    }

    public void SetDestination(Vector3 destination)
    {
        //this.destination = destination;
        agent.SetDestination(destination);
    }

    private void Update()
    {
        /*
        transform.position =
         Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        transform.LookAt(destination);
        */
    }
}
