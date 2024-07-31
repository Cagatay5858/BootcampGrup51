using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gear"))
        {
            GearCollect gearCollect = other.gameObject.GetComponent<GearCollect>();
            if (gearCollect != null)
            {
                gearCollect.Collect();
            }
        }
    }
}