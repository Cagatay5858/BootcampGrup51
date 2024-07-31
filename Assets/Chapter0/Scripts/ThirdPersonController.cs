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
    public float turnSmoothVelocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask dangerZoneMask;
    public Animator animator;
    public float interactionDistance = 2f;

    private Inventory inventory;
    private GameObject stick;
    private Vector3 velocity;
    private bool isGrounded;
    private NPCInteraction currentNPC;

    private FootstepManager footstepManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        footstepManager = GetComponent<FootstepManager>();

        
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);



        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Lying Down"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                OnLyingDownAnimationComplete();
            }
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        CheckDangerZone();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float speedParam = direction.magnitude;
        animator.SetFloat("Speed", speedParam);

        if (speedParam >= 0.1f)
        {
            if (isGrounded && !footstepManager.IsWalking())
            {
                footstepManager.StartWalking();
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            if (footstepManager.IsWalking())
            {
                footstepManager.StopWalking();
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.ResetTrigger("Jump");
            animator.ResetTrigger("RunningJump");

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
        CheckNPCDistance();
    }

    void CheckDangerZone()
    {
        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundDistance, dangerZoneMask);
        if (colliders.Length > 0)
        {
            RespawnPointSetter point = GetComponent<RespawnPointSetter>();
            if (point != null && point.getRespawnpoint() != null)
            {
                controller.enabled = false;
                transform.position = point.getRespawnpoint().position;
                transform.rotation = point.getRespawnpoint().rotation;
                controller.enabled = true;
                velocity = Vector3.zero;
                //can eksiltme eklenecek 
            }
        }
    }

    void CheckInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float sphereRadius = 0.5f;
            float interactionDistance = 5f;
            RaycastHit hit;
            Vector3 sphereCastOrigin = transform.position + Vector3.up * 1.2f;

            if (Physics.SphereCast(sphereCastOrigin, sphereRadius, transform.forward, out hit, interactionDistance))
            {
                NPCInteraction npcInteraction = hit.collider.GetComponent<NPCInteraction>();
                if (npcInteraction != null)
                {
                    if (currentNPC == npcInteraction)
                    {
                        npcInteraction.AdvanceDialogue();
                    }
                    else
                    {
                        if (currentNPC != null)
                        {
                            currentNPC.StopInteraction();
                        }

                        currentNPC = npcInteraction;
                        currentNPC.Interact();
                    }
                }
            }
        }
    }

    void CheckNPCDistance()
    {
        if (currentNPC != null)
        {
            float distance = Vector3.Distance(transform.position, currentNPC.transform.position);
            if (distance > interactionDistance)
            {
                currentNPC.StopInteraction();
                currentNPC = null;
            }
        }
    }

    private void OnLyingDownAnimationComplete()
    {
        SceneManager.Instance.OnLyingDownAnimationComplete();
    }
}

    

   
