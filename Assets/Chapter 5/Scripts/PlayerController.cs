using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    private Vector3 moveDirection;

    public float walkSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float aimSpeed = 1.0f; // Aim speed
    public float rotationSpeed = 700.0f;
    public float gravity = 9.81f;

    public CinemachineFreeLook freeLookCamera;
    public CinemachineVirtualCamera aimCamera;
    public Image crosshair;

    private bool isRunning;
    private bool isAiming;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        freeLookCamera.Priority = 10;
        aimCamera.Priority = 5;

        if (crosshair == null)
        {
            Debug.LogError("Crosshair UI element is not assigned.");
        }
        else
        {
            crosshair.gameObject.SetActive(false);
        }

        // Lock the mouse cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleAim();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        isRunning = Input.GetKey(KeyCode.LeftShift) && vertical > 0; // Only run forward
        float speed = isRunning ? runSpeed : walkSpeed;

        if (isAiming)
        {
            speed = aimSpeed;
        }

        // Prevent backward speed-up
        if (vertical < 0)
        {
            speed = walkSpeed;
        }

        // Ensure forward speed is higher than diagonal speeds
        if (vertical > 0 && horizontal != 0)
        {
            speed = runSpeed * 0.75f; // Diagonal speed
        }

        // Increase speeds and animation speeds for right and left movement
        if (horizontal != 0 && vertical == 0)
        {
            speed = runSpeed * 1.25f; // Increase side movement speed
            animator.speed = isRunning ? 1.75f : 1.25f; // Increase animation speed for side movement
        }

        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        moveDirection = transform.TransformDirection(moveDirection) * speed;

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        animator.SetFloat("x", horizontal);
        animator.SetFloat("y", vertical);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isAiming", isAiming);

        // Adjust animation speed based on movement direction
        if (horizontal < 0 || vertical < 0)
        {
            animator.speed = 1.5f; // Speed up when moving backwards or sideways
        }
        else
        {
            animator.speed = isRunning ? 1.5f : 1.0f; // Speed up when running, normal speed when walking
        }
    }

    void HandleRotation()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        if (mouseWorldPosition != Vector3.zero)
        {
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;
            direction.y = 0; // Keep the rotation only on the XZ plane
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    void HandleAim()
    {
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            isAiming = true;
            freeLookCamera.Priority = 5;
            aimCamera.Priority = 10;
            crosshair.gameObject.SetActive(true);
        }
        else
        {
            isAiming = false;
            freeLookCamera.Priority = 10;
            aimCamera.Priority = 5;
            crosshair.gameObject.SetActive(false);
        }
    }
}