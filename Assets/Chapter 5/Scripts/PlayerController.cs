using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public float speed = 5.0f;
    public float aimSpeed = 2.0f;
    public float aimSensitivity = 0.5f;
    public float followSensitivity = 0.5f;
    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera aimCamera;
    public GameObject crosshair;
    public Transform weapon;

    private bool isAiming = false;
    private Vector2 input;
    private CharacterController characterController;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;
    private float pitch = 0.0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        crosshair.SetActive(false);
        aimCamera.Priority = 9;
    }

    void Update()
    {
        HandleMovement();
        HandleAiming();
        if (isAiming)
        {
            AimWeaponAtCrosshair();
        }
    }

    void HandleMovement()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * 5f); 

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            characterController.Move(moveDirection * speed * Time.deltaTime);

            animator.SetFloat("x", input.x);
            animator.SetFloat("y", input.y);
        }
        else
        {
            animator.SetFloat("x", 0);
            animator.SetFloat("y", 0);
        }

        if (!isAiming) 
        {
            HandleMouseRotation();
        }
    }

    void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            aimCamera.Priority = 11;
            followCamera.Priority = 9;
            crosshair.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            aimCamera.Priority = 9;
            followCamera.Priority = 11;
            crosshair.SetActive(false);
        }

        if (isAiming)
        {
            HandleMouseRotation();
        }
    }

    void HandleMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * (isAiming ? aimSensitivity : followSensitivity);
        float mouseY = Input.GetAxis("Mouse Y") * (isAiming ? aimSensitivity : followSensitivity);

        transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -45f, 45f);

        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = pitch;
        Camera.main.transform.localEulerAngles = targetRotation;
    }

    void AimWeaponAtCrosshair()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPoint = hitInfo.point;
            weapon.LookAt(targetPoint);
        }
    }
}