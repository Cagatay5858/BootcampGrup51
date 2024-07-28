using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float attackDistance = 2.0f;
    public int damage = 10;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool isAttacking = false;
    public int maxHealth = 100;
    public int currentHealth;
    private HealthSystem healthSystem;
    private bool isDead = false;
    

    private void Start()
    {
        //awake func
        healthSystem = GetComponent<HealthSystemComponent>().GetHealthSystem();
        healthSystem.OnDead += HealthSystem_OnDead;
        animator = GetComponent<Animator>();
        //---//
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead) return; 
        if (player != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);

            if (distance <= attackDistance)
            {
                if (!isAttacking)
                {
                    StartAttack();
                }
            }
            else
            {
                StopAttack();
                navMeshAgent.SetDestination(player.position);
            }
        }
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        isDead = true;
        navMeshAgent.isStopped = true;
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

    private void StartAttack()
    {
        isAttacking = true;
        navMeshAgent.isStopped = true;
        animator.SetTrigger("Attack");
        InvokeRepeating("DealDamage", 0, 1.0f); 
    }

    private void StopAttack()
    {
        isAttacking = false;
        navMeshAgent.isStopped = false;
        animator.ResetTrigger("Attack");
        CancelInvoke("DealDamage");
    }

    private void DealDamage()
    {
        if (player != null)
        {
            HealthSystemComponent playerHealthSystemComponent = player.GetComponent<HealthSystemComponent>();
            if (playerHealthSystemComponent != null)
            {
                HealthSystem playerHealthSystem = playerHealthSystemComponent.GetHealthSystem();
                playerHealthSystem.Damage(damage);
                Debug.Log("Player Health: " + playerHealthSystem.GetHealth());
            }
        }
    }
}