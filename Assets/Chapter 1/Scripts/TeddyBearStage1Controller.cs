using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class TeddyBearStage1Controller : MonoBehaviour
{
    public static TeddyBearStage1Controller Instance;

    public CharacterController controller;
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
    public AudioClip jumpClip;

    private Inventory inventory;
    private Vector3 velocity;
    private bool isGrounded;

    private Stage1FootstepManager footstepManager;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        AssignCamera();

        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        footstepManager = GetComponent<Stage1FootstepManager>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignCamera();
    }

    private void AssignCamera()
    {
        CinemachineCameraManager.Instance.AssignCameraToPlayer(transform);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
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

            // Play the jump sound effect
            audioSource.PlayOneShot(jumpClip);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
}