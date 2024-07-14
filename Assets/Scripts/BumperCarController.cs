using UnityEngine;
using System.Collections;
public class BumperCarController : MonoBehaviour
{
    public float speed = 400f;          
    public float rotationSpeed = 5f; 
    public float bounceForce = 3f;    
    public string wallTag = "Wall";    
    public int damageAmount = 10;      
    public float slowMotionDuration = 2f; 
    public float slowMotionFactor = 0.4f; 
    public int maxHealth = 100;
    public int currentHealth;
    
    private Rigidbody rb;
    private Transform modelTransform;  
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY;
        modelTransform = transform.GetChild(0); 

        
        startPosition = transform.position;
        startRotation = transform.rotation;
        
        currentHealth = maxHealth;
    }

    void Update()
    {
        
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * moveInput * speed * Time.deltaTime;
        float turn = turnInput * rotationSpeed * Time.deltaTime;

        rb.AddForce(move, ForceMode.Force);
        rb.AddTorque(Vector3.up * turn, ForceMode.VelocityChange);
    }

    void FixedUpdate()
    {
      
        rb.position = new Vector3(rb.position.x, startPosition.y, rb.position.z);
        rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
           
            Vector3 contactNormal = collision.contacts[0].normal;         
            Vector3 bounce = contactNormal * bounceForce;
            rb.AddForce(bounce, ForceMode.Impulse);
        }
        else if (collision.gameObject.CompareTag("Car"))
        {
            
            BumperCarController otherCar = collision.gameObject.GetComponent<BumperCarController>();
            if (otherCar != null)
            {
                otherCar.TakeDamage(damageAmount);
            }

           
            TakeDamage(damageAmount);
        }
    }
     public void TakeDamage(int amount)
      {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
     }
     
     
     public void ActivateSlowMotion()
     {
         StartCoroutine(SlowMotionCoroutine());
     }

     private IEnumerator SlowMotionCoroutine()
     {
         Time.timeScale = slowMotionFactor;
         yield return new WaitForSecondsRealtime(slowMotionDuration);
         Time.timeScale = 1f;
     }
}