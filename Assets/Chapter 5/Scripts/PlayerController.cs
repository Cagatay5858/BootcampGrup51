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

        isRunning = Input.GetKey(KeyCode.LeftShift) && vertical > 0; 
        float speed = isRunning ? runSpeed : walkSpeed;

        if (isAiming)
        {
            speed = aimSpeed;
        }

        
        if (vertical < 0)
        {
            speed = walkSpeed * 2f;
        }
        
        if (vertical > 0 && horizontal != 0)
        {
            speed = runSpeed * 0.75f; 
        }

        
        if (horizontal != 0 && vertical == 0)
        {
            speed = runSpeed * 1.25f; 
            animator.speed = isRunning ? 1.75f : 1.25f; 
        }

        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        moveDirection = transform.TransformDirection(moveDirection) * speed;

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);

        animator.SetFloat("x", horizontal);
        animator.SetFloat("y", vertical);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isAiming", isAiming);

        
        if (horizontal < 0 || vertical < 0)
        {
            animator.speed = 1.5f;
            
        }
        else
        {
            animator.speed = isRunning ? 1.5f : 1.0f; 
        }
    }

    void HandleRotation()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        if (mouseWorldPosition != Vector3.zero)
        {
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;
            direction.y = 0; 
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
        if (Input.GetMouseButton(1)) 
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