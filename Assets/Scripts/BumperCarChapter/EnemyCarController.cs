using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyCarController : MonoBehaviour
{
    public Transform player;
    public float acceleration = 10f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 5f;
    public float bounceForce = 3f;
    public string wallTag = "Wall";
    public int damageAmount = 10;
    public float slowMotionDuration = 2f;
    public float slowMotionFactor = 0.4f;
    public int maxHealth = 100;
    public int currentHealth;

    public GameObject explosionPrefab; 

    private Rigidbody rb;
    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezePositionY;

        startPosition = transform.position;
        startRotation = transform.rotation;

        currentHealth = maxHealth;

        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    void FixedUpdate()
    {
        rb.position = new Vector3(rb.position.x, startPosition.y, rb.position.z);
        rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0);
        rb.velocity *= 0.98f;
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
            if (collision.gameObject.CompareTag("PlayerCar"))
            {
                BumperCarController playerCar = collision.gameObject.GetComponent<BumperCarController>();
                if (playerCar != null)
                {
                    Vector3 contactNormal = collision.contacts[0].normal;
                    Vector3 bounce = contactNormal * bounceForce;
                    rb.AddForce(bounce, ForceMode.Impulse);

                        //   if (!playerCar.isShieldActive)
                    {
                       // playerCar.TakeDamage(damageAmount);
                       // TakeDamage(damageAmount);
                    }
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Explode(); 
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

    private void Explode() 
    {
        if (explosionPrefab != null)
        {
            GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosionInstance, explosionInstance.GetComponent<ParticleSystem>().main.duration);
        }
    }
}