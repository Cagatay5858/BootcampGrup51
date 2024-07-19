using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidController : MonoBehaviour
{
    public float laneDistance = 2.0f; 
    public float jumpForce = 10.0f;
    public float gravity = -9.81f; 
    public Animator animator;

    private CharacterController controller;
    private Vector3 direction;
    private int targetLane = 1; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator.SetBool("isRunning", true); 
    }

    void Update()
    {
        
        direction.z = 10;

        
        if (controller.isGrounded)
        {
            direction.y = -1;
            animator.SetBool("isGrounded", true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
            animator.SetBool("isGrounded", false);
        }

        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLane(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLane(-1);
        }

      
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (targetLane == 0)
            targetPosition += Vector3.left * laneDistance;
        else if (targetLane == 2)
            targetPosition += Vector3.right * laneDistance;

        transform.position = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);

       
        controller.Move(direction * Time.deltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
        animator.SetTrigger("Jump");
    }

    private void MoveLane(int direction)
    {
        int targetLane = this.targetLane + direction;
        if (targetLane < 0 || targetLane > 2)
            return;

        this.targetLane = targetLane;

       
        if (direction == 1)
        {
            animator.SetTrigger("MoveRight");
        }
        else if (direction == -1)
        {
            animator.SetTrigger("MoveLeft");
        }
    }
}