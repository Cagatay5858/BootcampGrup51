using System.Collections;
using UnityEngine;

public class TeddyBearChaseController : MonoBehaviour
{
    public float laneDistance = 3.0f; 
    public float jumpForce = 10.0f;
    public float gravity = -9.81f; 
    public float speed = 10.0f; 
    public float laneSwitchSpeed = 10.0f;
    public Animator animator;
    public AudioSource maneuverAudioSource;
    public AudioClip rightManeuverClip;
    public AudioClip leftManeuverClip;
    private CharacterController controller;
    private Vector3 direction;
    private Vector3 targetPosition;
    private int targetLane = 1;
    private float initialYRotation;
    private float initialXPosition;

    private bool isShielded = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        initialYRotation = transform.eulerAngles.y;
        initialXPosition = transform.position.x;
        targetPosition = transform.position;
        SetAnimatorParameters(true, true);
    }

    void Update()
    {
        direction.z = speed;

        if (controller.isGrounded)
        {
            direction.y = -1;
            SetAnimatorParameters(true, true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                SetAnimatorParameters(false, false);
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
            SetAnimatorParameters(false, false);
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
        targetLane += direction;
        targetLane = Mathf.Clamp(targetLane, 0, 2);

        if (direction == 1)
        {
            animator.SetTrigger("MoveRight");

            maneuverAudioSource.clip = rightManeuverClip;
            maneuverAudioSource.Play();
        }
        else if (direction == -1)
        {
            animator.SetTrigger("MoveLeft");
            maneuverAudioSource.clip = leftManeuverClip;
            maneuverAudioSource.Play();
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
            if (isShielded)
            {
                Destroy(hit.gameObject);
                isShielded = false; 
            }
           
        }
        else if (hit.gameObject.CompareTag("ChasePowerUp"))
        {
            ChasePowerUp powerUp = hit.gameObject.GetComponent<ChasePowerUp>();
            if (powerUp != null)
            {
                powerUp.ApplyEffect(this);
                Destroy(hit.gameObject); 
            }
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
        animator.SetBool("isRunning", false);
        Time.timeScale = 0; 
    }

    
    public IEnumerator SpeedBoostCoroutine(float duration, float speedMultiplier)
    {
        speed *= speedMultiplier;
        yield return new WaitForSeconds(duration);
        speed /= speedMultiplier;
    }

    
    public IEnumerator ShieldCoroutine(float duration)
    {
        isShielded = true;
        yield return new WaitForSeconds(duration);
        isShielded = false;
        
    }
}