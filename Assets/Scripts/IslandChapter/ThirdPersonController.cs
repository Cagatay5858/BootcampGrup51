using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector3 velocity;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Animator animator;
    private Inventory inventory;
    private GameObject stick;

    public float interactionDistance = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float speedParam = direction.magnitude;
        animator.SetFloat("Speed", speedParam);

        if (speedParam >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            
            if (speedParam >= 0.1f)
            {
                animator.SetTrigger("RunningJump");
            }
            else
            {
                animator.SetTrigger("Jump");
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        CheckInteraction();
    }

    void CheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.position + Vector3.up * 1.5f, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                NPCInteraction npcInteraction = hit.collider.GetComponent<NPCInteraction>();
                if (npcInteraction != null)
                {
                    npcInteraction.Interact();
                    npcInteraction.GetComponent<NPCSpeechBubble>().ShowSpeechBubble("Hello, Player!"); // Örnek konuşma metni
                }
            }
        }
    }
}