using UnityEngine;

public class TeddyBearChaseController : MonoBehaviour
{
    public float laneDistance = 3.0f; 
    public float jumpForce = 10.0f;
    public float gravity = -9.81f; 
    public float speed = 10.0f; 
    public float laneSwitchSpeed = 10.0f;
    public Animator animator;
    public GameObject gameOverPanel; 
    private CharacterController controller;
    private Vector3 direction;
    private Vector3 targetPosition;
    private int targetLane = 1;
    private float initialYRotation;
    private float initialXPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        initialYRotation = transform.eulerAngles.y;
        initialXPosition = transform.position.x;
        targetPosition = transform.position;
        SetAnimatorParameters(true, true);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); 
        }
    }

    void Update()
    {
        direction.z = speed;

        if (controller.isGrounded)
        {
            if (direction.y < 0)
            {
                direction.y = -1;
            }
            SetAnimatorParameters(true, true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
            SetAnimatorParameters(false, true);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveLane(1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLane(-1);
        }

        UpdateTargetPosition();

        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, laneSwitchSpeed * Time.deltaTime);
        controller.Move(newPosition - transform.position);

        controller.Move(direction * Time.deltaTime);

        LockYRotation();
        LockXPosition();
    }

    private void Jump()
    {
        direction.y = jumpForce;
        animator.SetTrigger("Jump");
    }

    private void MoveLane(int direction)
    {
        if ((targetLane == 0 && direction == -1) || (targetLane == 2 && direction == 1))
        {
            return;
        }

        targetLane += direction;
        targetLane = Mathf.Clamp(targetLane, 0, 2);

        
        if (direction == 1 && targetLane != 2)
        {
            animator.SetTrigger("MoveRight");
        }
        else if (direction == -1 && targetLane != 0)
        {
            animator.SetTrigger("MoveLeft");
        }
    }

    private void UpdateTargetPosition()
    {
        targetPosition = transform.position.z * Vector3.forward;

        if (targetLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (targetLane == 2)
            targetPosition += Vector3.right * laneDistance;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Car"))
        {
            EndGame();
        }
    }

    private void LockYRotation()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, initialYRotation, transform.eulerAngles.z);
    }

    private void LockXPosition()
    {
        Vector3 position = transform.position;
        position.x = initialXPosition;
        transform.position = position;
    }

    private void SetAnimatorParameters(bool isGrounded, bool isRunning)
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isRunning", isRunning);
    }

    private void EndGame()
    {
        Debug.Log("Game Over");
        animator.SetBool("isRunning", false);
        Time.timeScale = 0; 

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); 
        }
    }
}