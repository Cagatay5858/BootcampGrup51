using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToyInteraction : MonoBehaviour
{
    public Transform handTransform;
    public float toyScaleFactor = 0.5f;
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.R;
    public float maxThrowForce = 50f;
    public float throwChargeTime = 1f;
    public Camera playerCamera;

    private GameObject currentToy = null;
    private Vector3 originalToyScale;
    private bool isHoldingToy = false;
    private Collider toyToPickup = null;
    private Rigidbody toyRigidbody = null;
    private float throwCharge = 0f;
    private bool isChargingThrow = false;

    public Animator animator;

    public Image chargeBarBG;
    public Image chargeBar;
    public float chargeBarWidthPercent = .3f;
    public float chargeBarHeightPercent = .015f;
    private CanvasGroup chargeBarCG;
    private float chargeBarWidth;
    private float chargeBarHeight;

    private Dictionary<GameObject, Vector3> toyOriginalScales = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        chargeBarWidth = screenWidth * chargeBarWidthPercent;
        chargeBarHeight = screenHeight * chargeBarHeightPercent;

        chargeBarBG.rectTransform.sizeDelta = new Vector3(chargeBarWidth, chargeBarHeight, 0f);
        chargeBar.rectTransform.sizeDelta = new Vector3(chargeBarWidth - 2, chargeBarHeight - 2, 0f);

        chargeBarCG = chargeBarBG.GetComponent<CanvasGroup>();
        chargeBarCG.alpha = 0;
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

                chargeBarCG.alpha = 1;
                float chargePercent = throwCharge / throwChargeTime;
                chargeBar.transform.localScale = new Vector3(chargePercent, 1f, 1f);
            }

            if (Input.GetKeyUp(dropKey))
            {
                DropToy();
                isChargingThrow = false;

                chargeBarCG.alpha = 0;
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
        if (!toyOriginalScales.ContainsKey(toy))
        {
            toyOriginalScales[toy] = toy.transform.localScale;
        }

        originalToyScale = toyOriginalScales[toy];
        toy.transform.localScale = originalToyScale * toyScaleFactor;
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
        if (toyRigidbody != null)
        {
            toyRigidbody.isKinematic = false;
            toyRigidbody.detectCollisions = true;

            float throwForce = (throwCharge / throwChargeTime) * maxThrowForce;
            Vector3 throwDirection = playerCamera.transform.forward;

            toyRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            toyRigidbody.drag = 0.5f;
            toyRigidbody.angularDrag = 0.05f;

            toyRigidbody = null;
        }

        currentToy.transform.SetParent(null);
        currentToy.transform.localScale = originalToyScale;
        currentToy = null;
        isHoldingToy = false;
        throwCharge = 0f;
    }
}