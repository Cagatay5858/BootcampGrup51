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
    

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
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

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            
        }
    }
    private void StartAttack()
    {
        isAttacking = true;
        navMeshAgent.isStopped = true;
        animator.SetTrigger("Attack"); 
        InvokeRepeating("DealDamage", 0, 1.0f); // Her 1 saniyede bir hasar verir
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
            
        }
    }

    void Die()
    {
        Debug.Log("Enemy dead");
        Destroy(gameObject);
    }
}