using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public Transform respawnpoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RespawnPointSetter pointSetter = other.gameObject.GetComponent<RespawnPointSetter>();
            if (pointSetter != null)
            {
                pointSetter.setRespawnpoint(respawnpoint);
                Debug.Log("Respawn point set to: " + respawnpoint.position);
            }
        }
    }
}