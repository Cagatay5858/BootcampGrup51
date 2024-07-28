using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ThrowEnemyAI : MonoBehaviour
{
    public Transform target;
    public float attackRange = 10f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isFiring;
    public int maxHealth = 100;
    public int currentHealth;
    private HealthSystem healthSystem;
    private bool isDead = false;

    private void Start()
    {
        // Awake-like initialization
        healthSystem = GetComponent<HealthSystemComponent>().GetHealthSystem();
        healthSystem.OnDead += HealthSystem_OnDead;
        animator = GetComponent<Animator>();
        // --- //
        agent = GetComponent<NavMeshAgent>();
        isFiring = false;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead) return;

        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        agent.SetDestination(target.position);

        if (distanceToTarget <= attackRange && Time.time >= nextFireTime)
        {
            StartCoroutine(FireCoroutine());
            nextFireTime = Time.time + fireRate;
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("dead");
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDead -= HealthSystem_OnDead;
        }
    }

    private IEnumerator FireCoroutine()
    {
        isFiring = true;

        while (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }

        isFiring = false;
    }

    private void Shoot()
    {
        animator.SetTrigger("Throw"); // Fırlatma animasyonunu tetikle

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 direction = (target.position - projectileSpawnPoint.position).normalized;
            rb.velocity = direction * 10f; // Hızını ayarlayabilirsiniz
            rb.AddTorque(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 10f, ForceMode.Impulse); // Yuvarlanma efekti
        }
    }
}
