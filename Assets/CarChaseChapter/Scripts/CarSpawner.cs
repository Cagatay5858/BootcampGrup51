using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; 
    public float spawnInterval = 2.0f; 
    public Transform player; 
    public float spawnDistance = 20.0f; 

    private void Start()
    {
        StartCoroutine(SpawnCars());
    }

    private IEnumerator SpawnCars()
    {
        while (true)
        {
            SpawnCar();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCar()
    {
        
        int laneIndex = Random.Range(0, 3);
        float laneOffset = (laneIndex - 1) * 3.0f; 

       
        Vector3 spawnPosition = player.position + new Vector3(laneOffset, 0, spawnDistance);
        Quaternion spawnRotation = Quaternion.Euler(0, 180, 0); 

        
        int carIndex = Random.Range(0, carPrefabs.Length); 
        Instantiate(carPrefabs[carIndex], spawnPosition, spawnRotation); 
    }
}