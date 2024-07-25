using UnityEngine;

public class ToyInteraction : MonoBehaviour
{
    public Transform handTransform; // The transform where the toy will be held
    public float toyScaleFactor = 0.5f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.R;
    public float maxThrowForce = 20f; // Maximum force applied to the toy when thrown
    public float throwChargeTime = 2f; // Time required to reach maximum throw force

    private GameObject currentToy = null;
    private Vector3 originalToyScale;
    private bool isHoldingToy = false;
    private Collider toyToPickup = null;
    private Rigidbody toyRigidbody = null; // Store the Rigidbody component
    private float throwCharge = 0f; // Charge amount for throw force
    private bool isChargingThrow = false; // Flag to track if charging throw

    void Update()
    {
        if (isHoldingToy)
        {
            // Ensure the toy stays fixed in the hand
            currentToy.transform.localPosition = Vector3.zero;
            currentToy.transform.localRotation = Quaternion.identity;

            // Handle throw charging
            if (Input.GetKey(dropKey))
            {
                isChargingThrow = true;
                throwCharge += Time.deltaTime;
                throwCharge = Mathf.Clamp(throwCharge, 0, throwChargeTime);
            }

            // Handle dropping the toy
            if (Input.GetKeyUp(dropKey))
            {
                DropToy();
                isChargingThrow = false;
            }
        }
        else
        {
            // Handle picking up the toy
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

        // Disable the Rigidbody
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
        // Check if the player is inside the drop zone
        if (IsInsideDropZone())
        {
            // Add points if inside drop zone
            // Implement your point system logic here
        }

        // Re-enable the Rigidbody
        if (toyRigidbody != null)
        {
            toyRigidbody.isKinematic = false;
            toyRigidbody.detectCollisions = true;

            // Calculate throw force
            float throwForce = (throwCharge / throwChargeTime) * maxThrowForce;
            Vector3 throwDirection = handTransform.forward; // Throw forward
            toyRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);

            toyRigidbody = null;
        }

        currentToy.transform.SetParent(null);
        currentToy.transform.localScale = originalToyScale;
        currentToy = null;
        isHoldingToy = false;
        throwCharge = 0f; // Reset throw charge
    }

    bool IsInsideDropZone()
    {
        // Implement logic to check if the player is inside the drop zone collider
        // This could be done with a trigger collider or by checking proximity to a specific object
        // Example:
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