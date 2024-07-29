using UnityEngine;

public class LargeCollider : MonoBehaviour
{
    private Bed bed;

    void Start()
    {
        bed = GetComponentInParent<Bed>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bed.isPlayerColliding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bed.isPlayerColliding = false;
        }
    }

    
}