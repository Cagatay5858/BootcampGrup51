using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private GameObject target;
    public float speed = 10f;
    public float lifespan = 5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rb.velocity = direction * speed; 
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target)
        {
            Destroy(target); 
            Destroy(gameObject); 
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}