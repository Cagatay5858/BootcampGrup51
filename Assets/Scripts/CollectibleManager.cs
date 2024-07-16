using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    ThirdPersonController thirdPersonController;
    Inventory Inventory;
    public bool inReach;
    public GameObject stick;
    public Animator animator;

    private void Start()
    {
        inReach = false;
        animator = GetComponent<Animator>();
        animator = thirdPersonController.animator;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trrrrrrrrriger");
        if (other.CompareTag("FootCollider"))
        {
            inReach = true;
        }
        else
        {
            //thirdPersonController.animator.SetBool("isLifting", false);
            inReach= false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FootCollider"))
        {
            inReach = false;
        }
    }
    private void OnDestroy()
    {
        inReach = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inReach)
        {
            Debug.Log("EEEEEEEEEEEEEEEEEE");
            animator.SetBool("isLifting", true);
            Inventory.AddStick();
            Destroy(this);
            animator.SetTrigger("PickUp");
            animator.SetBool("isLifting", false);

        }
        
    }
    void PickUpStick()
    {
            //thirdPersonController.animator.SetTrigger("PickUp");
            Inventory.AddStick();
            Destroy(this);
    }
}
