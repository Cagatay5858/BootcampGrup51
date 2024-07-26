using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class ThrowEnemyAI : MonoBehaviour
{
    public Transform target;
    public float attackRange = 10f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float fireRate = 100f;
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
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        agent.SetDestination(target.position);
        
        
        if (distanceToTarget <= attackRange && !isFiring)
        {
            StartCoroutine(FireCoroutine());
        }
    }

    IEnumerator FireCoroutine()
    {
        isFiring = true;

        while (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }

        isFiring = false;
    }

    void Shoot()
    {
        GameObject projectile =
            Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = (target.position - projectileSpawnPoint.position).normalized * 250f;
    }
    
    
}
