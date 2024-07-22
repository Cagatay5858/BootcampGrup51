using UnityEngine;
using UnityEngine.AI;

public class EnemyCarController : MonoBehaviour
{
    public Transform player; 
    public float followDistance = 10f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= followDistance)
        {
            agent.SetDestination(player.position);
        }
    }
}