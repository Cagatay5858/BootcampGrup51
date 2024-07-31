using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (hit.gameObject.CompareTag("Gear"))
        {
            GearCollect gearCollect = hit.gameObject.GetComponent<GearCollect>();
                gearCollect.Collect();
        }
    }
}