using System;
using UnityEngine;

public class ToyInteraction : MonoBehaviour
{
    public Transform handTransform; 
    public float toyScaleFactor = 0.5f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.R;
    public float maxThrowForce = 50f; 
    public float throwChargeTime = 1f; 

    private GameObject currentToy = null;
    private Vector3 originalToyScale;
    private bool isHoldingToy = false;
    private Collider toyToPickup = null;
    private Rigidbody toyRigidbody = null;
    private float throwCharge = 0f;
    private bool isChargingThrow = false;
    
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

            if (Input.GetKey(dropKey))
            {
                isChargingThrow = true;
                throwCharge += Time.deltaTime;
                throwCharge = Mathf.Clamp(throwCharge, 0, throwChargeTime);
            }

            if (Input.GetKeyUp(dropKey))
            {
                DropToy();
                isChargingThrow = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(pickupKey) && toyToPickup != null)
            {
                PickupToy(toyToPickup.gameObject);
            }
        }
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

        animator.SetBool("isHoldingToy", true); 
    }

    void DropToy()
    {
        if (IsInsideDropZone())
        {
            
        }

        if (toyRigidbody != null)
        {
            toyRigidbody.isKinematic = false;
            toyRigidbody.detectCollisions = true;

            float throwForce = (throwCharge / throwChargeTime) * maxThrowForce;
            Vector3 throwDirection = handTransform.forward;

            toyRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            toyRigidbody.drag = 0.1f; 
            toyRigidbody.angularDrag = 0.01f; 

            toyRigidbody = null;
        }

        currentToy.transform.SetParent(null);
        currentToy.transform.localScale = originalToyScale;
        currentToy = null;
        isHoldingToy = false;
        throwCharge = 0f;

        animator.SetBool("isHoldingToy", false); 
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