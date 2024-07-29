using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FPSBearController : MonoBehaviour
{
    private Rigidbody rb;
    
    public Transform player;
    public Camera playerCamera;
    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;
    public bool lockCursor = true;
    public bool crosshair = true;
    public Sprite crosshairImage;
    public Color crosshairColor = Color.white;
    public float extraGravity = 2.0f;
    public GameObject gameOverUI;

    private Vector3 initialCameraPosition;

    public int playerHealth = 100;
    
    //Fire Crosshair
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public KeyCode fireKey = KeyCode.Mouse0;
    
    private bool isFiring = false;
    private float nextFireTime = 0f;
    private Animator animator; 
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;
    private Coroutine fireCoroutine;
    
    public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;
    private bool isZoomed = false;
    
    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;
    private bool isWalking = false;
    
    
    public bool enableSprint = true;
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 7f;
    public float sprintDuration = 5f;
    public float sprintCooldown = .5f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;
    public bool useSprintBar = true;
    public bool hideBarWhenFull = true;
    public Image sprintBarBG;
    public Image sprintBar;
    public float sprintBarWidthPercent = .3f;
    public float sprintBarHeightPercent = .015f;
  
    private CanvasGroup sprintBarCG;
    private bool isSprinting = false;
    private float sprintRemaining;
    private float sprintBarWidth;
    private float sprintBarHeight;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;
    
    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;
    private bool isGrounded = false;
    
    
    private Vector3 originalScale;
    public Transform joint;
    private Vector3 jointOriginalPos;
    private float timer = 0;
    
    
    public float walkBobSpeed = 14f;
    public float walkBobAmount = 0.05f;
    public float sprintBobSpeed = 18f;
    public float sprintBobAmount = 0.1f;
    private float defaultYPos = 0;
    private float headBobTimer = 0;
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        crosshairObject = GetComponentInChildren<Image>();
        
        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;
        //jointOriginalPos = joint.localPosition;

        if (!unlimitedSprint)
        {
            sprintRemaining = sprintDuration;
            sprintCooldownReset = sprintCooldown;
        }
    }

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
        animator = GetComponent<Animator>();

        initialCameraPosition = playerCamera.transform.localPosition;
        
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if(crosshair)
        {
            crosshairObject.sprite = crosshairImage;
            crosshairObject.color = crosshairColor;
        }
        else
        {
            crosshairObject.gameObject.SetActive(false);
        }
        
        sprintBarCG = GetComponentInChildren<CanvasGroup>();

        if(useSprintBar)
        {
            sprintBarBG.gameObject.SetActive(true);
            sprintBar.gameObject.SetActive(true);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            sprintBarWidth = screenWidth * sprintBarWidthPercent;
            sprintBarHeight = screenHeight * sprintBarHeightPercent;

            sprintBarBG.rectTransform.sizeDelta = new Vector3(sprintBarWidth, sprintBarHeight, 0f);
            sprintBar.rectTransform.sizeDelta = new Vector3(sprintBarWidth - 2, sprintBarHeight - 2, 0f);

            if(hideBarWhenFull)
            {
                sprintBarCG.alpha = 0;
            }
        }
        else
        {
            sprintBarBG.gameObject.SetActive(false);
            sprintBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        CheckGround();
        animator.SetBool("isFiring", isFiring);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isSprinting", isSprinting);
        animator.SetBool("isGrounded", isGrounded);
        
        if(cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

            if (!invertCamera)
            {
                pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            }
            else
            {
                pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
            }

            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
            
        }

        if (enableZoom)
        {
            if(Input.GetKeyDown(zoomKey) && !holdToZoom && !isSprinting)
            {
                isZoomed = !isZoomed;
            }

            if(holdToZoom && !isSprinting)
            {
                if(Input.GetKeyDown(zoomKey))
                {
                    isZoomed = true;
                }
                else if(Input.GetKeyUp(zoomKey))
                {
                    isZoomed = false;
                }
            }

            if(isZoomed)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            }
            else if(!isZoomed && !isSprinting)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomStepTime * Time.deltaTime);
            }
        }

        if(enableSprint)
        {
            if(isSprinting)
            {
                isZoomed = false;
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);

                if(!unlimitedSprint)
                {
                    sprintRemaining -= 1 * Time.deltaTime;
                    if (sprintRemaining <= 0)
                    {
                        isSprinting = false;
                        isSprintCooldown = true;
                    }
                }
            }
            else
            {
                sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
            }

            if(isSprintCooldown)
            {
                sprintCooldown -= 1 * Time.deltaTime;
                if (sprintCooldown <= 0)
                {
                    isSprintCooldown = false;
                }
            }
            else
            {
                sprintCooldown = sprintCooldownReset;
            }

            if(useSprintBar && !unlimitedSprint)
            {
                float sprintRemainingPercent = sprintRemaining / sprintDuration;
                sprintBar.transform.localScale = new Vector3(sprintRemainingPercent, 1f, 1f);
            }
        }
        
        if (isWalking || isSprinting)
        {
            headBobTimer += Time.deltaTime * (isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                initialCameraPosition.x,
                initialCameraPosition.y + Mathf.Sin(headBobTimer) * (isSprinting ? sprintBobAmount : walkBobAmount),
                initialCameraPosition.z
            );
        }
        else
        {
            headBobTimer = 0;
            playerCamera.transform.localPosition = new Vector3(
                initialCameraPosition.x,
                Mathf.Lerp(playerCamera.transform.localPosition.y, initialCameraPosition.y, Time.deltaTime * (isSprinting ? sprintBobSpeed : walkBobSpeed)),
                initialCameraPosition.z
            );
        }
        
        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(fireKey))
        {
            if(fireCoroutine == null)
            {
               fireCoroutine= StartCoroutine(FireContinously());
            }
            
        }

        if(Input.GetKeyUp(fireKey))
        {
            if(fireCoroutine != null)
                StopCoroutine(fireCoroutine);
            fireCoroutine = null;
            isFiring = false;
        }

    }

    void FixedUpdate()
    {
        CheckGround();
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Impulse);
        }
        if (playerCanMove)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, Time.deltaTime);
           
            if (targetVelocity != Vector3.zero && !isSprinting)
            {
                isWalking = true;
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;
            }
            else
            {
                isWalking = false;
            }

            if (enableSprint && Input.GetKey(sprintKey) && sprintRemaining > 0f && !isSprintCooldown)
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                if (velocityChange.x != 0 || velocityChange.z != 0)
                {
                    isSprinting = true;
                    
                    if (hideBarWhenFull && !unlimitedSprint)
                    {
                        sprintBarCG.alpha += 5 * Time.deltaTime;
                    }
                }

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            else
            {
                isSprinting = false;

                if (hideBarWhenFull && sprintRemaining == sprintDuration)
                {
                    sprintBarCG.alpha -= 3 * Time.deltaTime;
                }

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }

      
        
    }


    private void CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
    
        float raycastDistance = (transform.localScale.y * 0.0072f) + 0.1f;
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * ((transform.localScale.y * 0.0072f) + 0.1f));

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position - new Vector3(0, (transform.localScale.y * 0.5f) + 0.1f, 0), 0.1f);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = playerCamera.transform.forward * 40f;

        
        Vector3 randomTorque = new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f)
        ) * 10f; 
        bulletRb.AddTorque(randomTorque, ForceMode.Impulse);
    }
    public void TakeDamage(int damage)
    {
        HealthSystemComponent playerHealthSystemComponent = player.GetComponent<HealthSystemComponent>();
        HealthSystem playerHealthSystem = playerHealthSystemComponent.GetHealthSystem();
        
            if (playerHealthSystem.GetHealth() <= 0)
            {
                Die();
            }
    }
    
    
    private void Die()
    {
        Time.timeScale = 0f;
        if (gameOverUI != null )
        {
            gameOverUI.SetActive(true);
        }
    }

    private IEnumerator FireContinously()
    {
        while (true)
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                isFiring = true;
                Fire();
            }

            yield return null;
        }
    }
}



