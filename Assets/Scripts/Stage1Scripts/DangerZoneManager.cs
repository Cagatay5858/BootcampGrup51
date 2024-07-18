using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneManager : MonoBehaviour
{

    public Transform playerTransform;
    public Transform defaultSpawnPoint;

    private void OnCollisionEnter(Collision collision)
    {
        RespawnPointSetter point = new RespawnPointSetter();

        if (collision.gameObject.CompareTag("Player"))
        {
            if(point.getRespawnpoint() != null)
            {
                playerTransform = defaultSpawnPoint;
                //burda da default baslangic noktasinda atacak ve yine can -1 yapacak
            }
            else
            {
                playerTransform = point.transform;
                //Can -1 falan olacak burda
            }
        }
            
    }
}
