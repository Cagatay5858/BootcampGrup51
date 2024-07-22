using UnityEngine;
using Cinemachine;

public class ThirdPersonShooterController : MonoBehaviour
{
    public float walkSpeedForward = 3.0f;
    public float walkSpeedBackward = 2.0f;
    public float strafeSpeed = 2.0f;
    public float runSpeed = 6.0f;
    public float jumpHeight = 1.0f;
    public float gravity = -9.81f;
    public Animator animator;
    public Transform cameraTransform;
    public Transform handTransform;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    public float mouseSensitivity = 100f;
    public float fireRate = 0.1f;
    private float nextTimeToFire = 0f;

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float yRotation = 0f;
    private float xRotation = 0f;
    private bool isAiming = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleAimingAndShooting();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 60f);

        yRotation += mouseX;

        cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);  
    }

    private void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            animator.SetBool("isJumping", false);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isWalkingBackwards = vertical < 0;
        animator.SetBool("isWalkingBackwards", isWalkingBackwards);

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDirection = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * direction;
            float speed = 0f;

            if (vertical > 0)
            {
                speed = walkSpeedForward;
            }
            else if (vertical < 0)
            {
                speed = walkSpeedBackward;
            }
            else if (horizontal != 0)
            {
                speed = strafeSpeed;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }

            controller.Move(moveDirection * speed * Time.deltaTime);

            animator.SetFloat("Speed", speed);
            animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));
        }
        else
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("isRunning", false);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }
    }

    private void HandleAimingAndShooting()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (Input.GetButton("Fire2"))
        {
            isAiming = true;
            aimVirtualCamera.gameObject.SetActive(true);
            mouseSensitivity = aimSensitivity;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 13f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            isAiming = false;
            aimVirtualCamera.gameObject.SetActive(false);
            mouseSensitivity = normalSensitivity;
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 13f));
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot(mouseWorldPosition, hitTransform);
            animator.SetBool("isShooting", true);
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("isShooting", false);
        }

        animator.SetBool("isAiming", isAiming);
    }

    void Shoot(Vector3 mouseWorldPosition, Transform hitTransform)
    {
        if (hitTransform != null)
        {
            if (hitTransform.GetComponent<BulletTarget>() != null)
            {
                Instantiate(vfxHitGreen, mouseWorldPosition, Quaternion.identity);
            }
            else
            {
                Instantiate(vfxHitRed, mouseWorldPosition, Quaternion.identity);
            }
        }

        GameObject bullet = Instantiate(bulletPrefab, handTransform.position, handTransform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = handTransform.forward * bulletSpeed;
    }
}