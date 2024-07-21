using UnityEngine;

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
        // Mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 60f);

        yRotation += mouseX;

        cameraTransform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

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
            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        
        if (Input.GetButton("Fire2"))
        {
            isAiming = true;
            animator.SetBool("isAiming", true);
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isAiming = false;
            animator.SetBool("isAiming", false);
        }

     
        if (isAiming)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                animator.SetBool("isShooting", true);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                animator.SetBool("isShooting", false);
            }
        }
        else
        {
            animator.SetBool("isShooting", false);
        }

        
        animator.SetBool("isStrafingLeft", horizontal < 0);
        animator.SetBool("isStrafingRight", horizontal > 0);

       
        animator.SetBool("isWalking", direction.magnitude >= 0.1f && isAiming);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, handTransform.position, handTransform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = handTransform.forward * bulletSpeed;
    }
}