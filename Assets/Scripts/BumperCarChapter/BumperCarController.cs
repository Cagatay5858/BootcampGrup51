using UnityEngine;
using System.Collections;

public class BumperCarController : MonoBehaviour
{
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
    public bool isShieldActive = false;
    public GameObject projectilePrefab;
    public float projectileSpeed = 2f;

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

        Vector3 moveDirection = transform.forward * moveInput * acceleration;

        rb.AddForce(moveDirection);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        float turn = turnInput * rotationSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * turn));
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
            EnemyCarController otherCar = collision.gameObject.GetComponent<EnemyCarController>();
            if (otherCar != null)
            {
                Vector3 contactNormal = collision.contacts[0].normal;
                Vector3 bounce = contactNormal * bounceForce;
                rb.AddForce(bounce, ForceMode.Impulse);

                if (!isShieldActive)
                {
                    otherCar.TakeDamage(damageAmount);
                    TakeDamage(damageAmount);
                }
            }
        }
    }
    public GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Car");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
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
}