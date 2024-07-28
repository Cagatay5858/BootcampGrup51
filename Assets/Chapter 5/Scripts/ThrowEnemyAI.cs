using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThrowEnemyAI : MonoBehaviour
{
    public Transform target;
    public float attackRange = 10f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float fireRate = 1f; // Saniyede kaç kere ateş edilecek
    private float nextFireTime = 0f;

    private NavMeshAgent agent;
    private bool isFiring;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isFiring = false;
    }

    private void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        agent.SetDestination(target.position);

        if (distanceToTarget <= attackRange && Time.time >= nextFireTime)
        {
            StartCoroutine(FireCoroutine());
            nextFireTime = Time.time + fireRate;
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