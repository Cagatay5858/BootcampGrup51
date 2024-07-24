using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;

public class EnemyCarController : MonoBehaviour
{
    public Transform player;
    public string EnemysName;
    public float acceleration = 10f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 5f;
    public float bounceForce = 3f;
    public string wallTag = "Wall";
    public string carTag = "Car";
    public string playerCarTag = "PlayerCar";
    public int damageAmount = 10;
    public float slowMotionDuration = 2f;
    public float slowMotionFactor = 0.4f;
    public int maxHealth = 100;
    public int currentHealth;

    public TMPro.TMP_Text healthUI;

    public GameObject explosionPrefab;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private bool isAccelerating = false;
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;
    private float randomChangeTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezePositionY;

        startPosition = transform.position;
        startRotation = transform.rotation;

        currentHealth = maxHealth;
        var cHstring = currentHealth.ToString();
        healthUI.text = EnemysName + " Health : " + cHstring;

        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            agent.SetDestination(player.position + randomOffset);

            if (Time.time > randomChangeTime)
            {
                targetSpeed = Random.Range(0.5f * maxSpeed, maxSpeed);
                randomChangeTime = Time.time + Random.Range(2f, 5f);
            }

           
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * acceleration);
            agent.speed = currentSpeed;
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
        else if (collision.gameObject.CompareTag(carTag) || collision.gameObject.CompareTag(playerCarTag))
        {
            Vector3 contactNormal = collision.contacts[0].normal;
            Vector3 bounce = contactNormal * bounceForce;
            rb.AddForce(bounce, ForceMode.Impulse);

            
            Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
            if (otherRb != null)
            {
                Vector3 otherBounce = -contactNormal * bounceForce;
                otherRb.AddForce(otherBounce, ForceMode.Impulse);
            }

            if (collision.gameObject.CompareTag(playerCarTag))
            {
                BumperCarController playerCar = collision.gameObject.GetComponent<BumperCarController>();
                if (playerCar != null)
                {
                    if (!playerCar.isShieldActive)
                    {
                        playerCar.TakeDamage(damageAmount);
                        TakeDamage(damageAmount);
                    }
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        var cHString = currentHealth.ToString();
        healthUI.text = EnemysName + " Health : " + cHString;
        if (currentHealth <= 0)
        {
            Explode();
            Destroy(gameObject);
        }

       FindObjectOfType<HealthUIController>().UpdateHealth(gameObject, currentHealth, maxHealth);
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