using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}