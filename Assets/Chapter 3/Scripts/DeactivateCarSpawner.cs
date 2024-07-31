using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCarSpawner : MonoBehaviour
{
    public GameObject carSpawner;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Oyuncu collider'a çarptığında CarSpawner objesini deaktif et
            carSpawner.SetActive(false);
        }
    }
}