using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float aimSpeed = 2f;
    public Transform aimTarget;
    public Camera mainCamera;

    private Animator animator;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private bool isAiming = false;
    private bool isRunning = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        HandleAiming();
        UpdateAnimator();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        isRunning = Input.GetKey(KeyCode.LeftShift); // Shift tuşuna basıldığında koş

        float speed = isRunning ? runSpeed : walkSpeed;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            Vector3 direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(direction * speed * Time.deltaTime);
        }
    }

    void HandleAiming()
    {
        if (Input.GetMouseButton(1)) // Sağ fare tuşu ile nişan alma
        {
            isAiming = true;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                aimTarget.position = hit.point;
                Vector3 direction = (aimTarget.position - transform.position).normalized;
                direction.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * aimSpeed);
            }
        }
        else
        {
            isAiming = false;
        }
    }

    void UpdateAnimator()
    {
        bool isMoving = moveDirection.magnitude >= 0.1f;
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isAiming", isAiming);
        animator.SetBool("isRunning", isRunning);
    }
}