using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



[RequireComponent(typeof(CharacterController))]

public class Layer : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    private bool isGrounded;
    public float moveSpeed = 5.0f;
    public LayerMask groundLayer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        CheckGrounded();
    }

    void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        controller.Move(move);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void CheckGrounded()
    {
        // CharacterController'ýn yere temas edip etmediðini kontrol edin
        isGrounded = controller.isGrounded;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}

        
    

