using UnityEngine;

public class TeddyBearChaseController : MonoBehaviour
{
   
    public float laneDistance = 3.0f; 
    public float jumpForce = 10.0f;
    public float gravity = -9.81f; 
    public float speed = 10.0f; 
    public Animator animator;

    private CharacterController controller;
    private Vector3 direction;
    private int targetLane = 1;
    private float initialYRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator.SetBool("isRunning", true);

        initialYRotation = transform.eulerAngles.y;

    }

    void Update()
    {
        direction.z = speed;

        if (controller.isGrounded)
        {
            direction.y = -1; 
            animator.SetBool("isGrounded", true);
            animator.SetBool("isRunning", true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                animator.SetBool("isGrounded", false);
                animator.SetBool("isRunning", false);
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
            animator.SetBool("isGrounded", false);
            animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveLane(1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLane(-1);
        }

        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (targetLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (targetLane == 2)
            targetPosition += Vector3.right * laneDistance;

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        Vector3 moveAmount = moveDirection * speed * Time.deltaTime;
        controller.Move(moveAmount);

        controller.Move(direction * Time.deltaTime);
        
       LockYRotation();
    }

    private void Jump()
    {
        direction.y = jumpForce;
        animator.SetTrigger("Jump");
    }

    private void MoveLane(int direction)
    {
        targetLane += direction;
        targetLane = Mathf.Clamp(targetLane, 0, 2);

        if (direction == 1)
        {
            animator.SetTrigger("MoveRight");
        }
        else if (direction == -1)
        {
            animator.SetTrigger("MoveLeft");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Car"))
        {
           
            Debug.Log("Game Over");
           
        }
    }

    private void LockYRotation()
    {
        Vector3 currentRotation = transform.eulerAngles;
        transform.eulerAngles = new Vector3(currentRotation.x, initialYRotation, currentRotation.z);
    }
}