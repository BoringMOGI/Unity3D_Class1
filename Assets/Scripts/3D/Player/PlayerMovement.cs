using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;                   // �̵� �ӵ�. (�Ÿ�)
    [SerializeField] float jumpHeight;                  // ���� ����.

    [Range(1.0f, 3.0f)]
    [SerializeField] float gravityScale;                // �߷� ���ӵ��� ����.

    [SerializeField] Transform groundChecker;           // ���� üũ ��ġ.
    [SerializeField] float groundRadius;                // ���� üũ ������.
    [SerializeField] LayerMask groundMask;              // ���� ����ũ.

    [SerializeField] Animator anim;                     // �ִϸ��̼�.
    [SerializeField] Footstep footstep;                 // �߼Ҹ�.
    

    CharacterController controller;                     // ĳ���� ��Ʈ�ѷ�.

    float gravity => -9.81f * gravityScale;
    Vector3 velocity;                                   // �ϰ� �ӵ�.
    bool isGrounded;                                    // ���� ����.
    bool isAlive;                                       // ���� ����.

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        isAlive = true;
    }

    private void Update()
    {
        CheckGround();

        #if UNITY_STANDALONE
        if (isAlive)
        {
            Movement();
            Jump();
        }
        #endif

        Gravity();
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundRadius, groundMask);
    }
    void Gravity()
    {
        // �߷�.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void Movement()
    {
        bool isAccel = Input.GetKey(KeyCode.LeftShift);

        float x = Input.GetAxisRaw("Horizontal");  // Ű���� ��,�� Ű.
        float z = Input.GetAxisRaw("Vertical");    // Ű���� ��,�� Ű.
        float accel = isAccel ? 1.5f : 1.0f;
        Vector3 direction = (transform.right * x) + (transform.forward * z);

        bool isWalk = direction != Vector3.zero && !isAccel;
        bool isRun = direction != Vector3.zero && isAccel;

        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isRun", isRun);

        controller.Move(direction * moveSpeed * accel * Time.deltaTime);
        footstep.OnPlay(isWalk, isRun);
    }
    public void OnMoveJoystick(Vector2 movement)
    {
        bool isAccel = false;

        float x = movement.x;
        float z = movement.y;
        float accel = isAccel ? 1.5f : 1.0f;

        Vector3 direction = (transform.right * x) + (transform.forward * z);
        bool isWalk = direction != Vector3.zero && !isAccel;
        bool isRun = direction != Vector3.zero && isAccel;

        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isRun", isRun);

        controller.Move(direction * moveSpeed * accel * Time.deltaTime);
        footstep.OnPlay(isWalk, isRun);
    }


    void Jump()
    {
        // ����.
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    public void OnJumpJoystick()
    {
        // ����.
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    

    public void OnDead()
    {
        isAlive = false;
    }
}
