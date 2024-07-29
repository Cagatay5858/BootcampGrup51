using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneManager : MonoBehaviour
{
    public Transform defaultSpawnPoint; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered danger zone");
            RespawnPointSetter point = other.gameObject.GetComponent<RespawnPointSetter>();

            if (point == null)
            {
                Debug.LogError("RespawnPointSetter component not found on the player!");
                other.transform.position = defaultSpawnPoint.position;
                other.transform.rotation = defaultSpawnPoint.rotation;
                Debug.Log("Player respawned at default spawn point.");
            }
            else if (point.getRespawnpoint() == null)
            {
                Debug.LogError("Respawn point is not set in RespawnPointSetter!");
                other.transform.position = defaultSpawnPoint.position;
                other.transform.rotation = defaultSpawnPoint.rotation;
                Debug.Log("Player respawned at default spawn point.");
            }
            else
            {
                Debug.Log("Custom respawn point found");
                other.transform.position = point.getRespawnpoint().position;
                other.transform.rotation = point.getRespawnpoint().rotation;
                Debug.Log("Player respawned at custom respawn point.");
            }
        }
    }
}