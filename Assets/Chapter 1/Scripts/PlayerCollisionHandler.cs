using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collision detected with: " + hit.gameObject.name); // Debug mesajı eklendi
        if (hit.gameObject.CompareTag("Gear"))
        {
            Debug.Log("Player collided with Gear."); // Debug mesajı eklendi
            GearCollect gearCollect = hit.gameObject.GetComponent<GearCollect>();
                gearCollect.Collect();
        }
    }
}