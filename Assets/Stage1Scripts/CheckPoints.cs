using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{

    public Transform respawnpoint;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RespawnPointSetter pointSetter = new RespawnPointSetter();
            pointSetter.setRespawnpoint(respawnpoint);
        }
    }
}
