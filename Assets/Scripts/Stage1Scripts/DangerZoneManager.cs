using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneManager : MonoBehaviour
{

    public Transform playerTransform; // Reference to the player's transform
    public Transform defaultSpawnPoint; // Reference to the default spawn point

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RespawnPointSetter point = collision.gameObject.GetComponent<RespawnPointSetter>();

            if (point == null)
            {
                Debug.LogError("RespawnPointSetter component not found on the player!");
                // Use the default spawn point if RespawnPointSetter component is not found
                collision.transform.position = defaultSpawnPoint.position;
                collision.transform.rotation = defaultSpawnPoint.rotation;
                // Apply any penalty here (e.g., reduce player's health)
                Debug.Log("Player respawned at default spawn point.");
            }
            else if (point.getRespawnpoint() == null)
            {
                Debug.LogError("Respawn point is not set in RespawnPointSetter!");
                // Use the default spawn point if no respawn point is set
                collision.transform.position = defaultSpawnPoint.position;
                collision.transform.rotation = defaultSpawnPoint.rotation;
                // Apply any penalty here (e.g., reduce player's health)
                Debug.Log("Player respawned at default spawn point.");
            }
            else
            {
                // Use the custom respawn point if set
                collision.transform.position = point.getRespawnpoint().position;
                collision.transform.rotation = point.getRespawnpoint().rotation;
                // Apply any penalty here (e.g., reduce player's health)
                Debug.Log("Player respawned at custom respawn point.");
            }
        }
    }
}
