using System;
using UnityEngine;

public class ToyInteraction : MonoBehaviour
{
    public Transform handTransform; 
    public float toyScaleFactor = 0.5f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.E;
    public int pointsPerToy = 10; 
    private int score = 0;

    private GameObject currentToy = null;
    private Vector3 originalToyScale;
    private bool isHoldingToy = false;
    private Collider toyToPickup = null;
    private Rigidbody toyRigidbody = null; 
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
       
    }

    void Update()
    {
        if (isHoldingToy)
        {
           
            currentToy.transform.localPosition = Vector3.zero;
            currentToy.transform.localRotation = Quaternion.identity;

            
            if (Input.GetKeyDown(dropKey))
            {
                DropToy();
            }
        }
        else
        {
            
            if (Input.GetKeyDown(pickupKey) && toyToPickup != null)
            {
                PickupToy(toyToPickup.gameObject);
            }
        }
        animator.SetBool("isHoldingToy", isHoldingToy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Toy") && !isHoldingToy)
        {
            toyToPickup = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Toy") && other == toyToPickup)
        {
            toyToPickup = null;
        }
    }

    void PickupToy(GameObject toy)
    {
        currentToy = toy;
        originalToyScale = toy.transform.localScale;
        toy.transform.localScale *= toyScaleFactor;
        toy.transform.SetParent(handTransform);
        toy.transform.localPosition = Vector3.zero;
        toy.transform.localRotation = Quaternion.identity;

        
        toyRigidbody = toy.GetComponent<Rigidbody>();
        if (toyRigidbody != null)
        {
            toyRigidbody.isKinematic = true;
            toyRigidbody.detectCollisions = false;
        }

        isHoldingToy = true;
        toyToPickup = null;
    }

    void DropToy()
    {
        
        if (IsInsideDropZone())
        {
           
            score += pointsPerToy;
            Debug.Log("Points earned! Current score: " + score);
        }

        
        if (toyRigidbody != null)
        {
            toyRigidbody.isKinematic = false;
            toyRigidbody.detectCollisions = true;
            toyRigidbody = null;
        }

        currentToy.transform.SetParent(null);
        currentToy.transform.localScale = originalToyScale;
        currentToy = null;
        isHoldingToy = false;
    }

    bool IsInsideDropZone()
    {
       
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("DropZone"))
            {
                return true;
            }
        }
        return false;
    }
}