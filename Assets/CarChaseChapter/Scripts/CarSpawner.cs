using System.Collections;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; 
    public GameObject mcQueenPrefab; 
    public float spawnInterval = 2.0f;
    public Transform player;
    public float spawnDistance = 20.0f;
    public float mcQueenSpawnChance = 0.1f;

    private Vector3[] lanePositions;

    private void Start()
    {
        lanePositions = new Vector3[3];
        lanePositions[0] = new Vector3(-3, 0, spawnDistance);
        lanePositions[1] = new Vector3(0, 0, spawnDistance);
        lanePositions[2] = new Vector3(3, 0, spawnDistance);

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
        int laneIndex = Random.Range(0, lanePositions.Length);
        Vector3 spawnPosition = player.position + lanePositions[laneIndex];
        Quaternion spawnRotation = Quaternion.Euler(0, 180, 0);

        GameObject carToSpawn;
        if (Random.value < mcQueenSpawnChance)
        {
            carToSpawn = mcQueenPrefab; 
        }
        else
        {
            int carIndex = Random.Range(0, carPrefabs.Length);
            carToSpawn = carPrefabs[carIndex]; 
        }

        Instantiate(carToSpawn, spawnPosition, spawnRotation);
    }
}