using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;
    public float spawnInterval = 10.0f;
    public Transform player;
    public float spawnDistance = 30.0f;
    public float spawnHeight = 0.6f; 

    private void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            SpawnPowerUp();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnPowerUp()
    {
        int laneIndex = Random.Range(0, 3);
        float laneOffset = (laneIndex - 1) * 3.0f;
        Vector3 spawnPosition = player.position + new Vector3(laneOffset, 0, spawnDistance);
        spawnPosition.y = spawnHeight; 

        int powerUpIndex = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[powerUpIndex], spawnPosition, Quaternion.identity);
    }
}