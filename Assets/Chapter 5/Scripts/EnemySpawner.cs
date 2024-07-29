using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject throwEnemyPrefab;
    public GameObject meleeEnemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    public Transform player;  
    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy(throwEnemyPrefab);
            SpawnEnemy(meleeEnemyPrefab);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

       
        float scale = Random.Range(0.5f, 2f);
        enemy.transform.localScale = new Vector3(scale, scale, scale);

        
        HealthSystemComponent healthSystemComponent = enemy.GetComponent<HealthSystemComponent>();
        if (healthSystemComponent != null)
        {
            HealthSystem healthSystem = healthSystemComponent.GetHealthSystem();
            healthSystem.SetMaxHealth(scale * 100); 
        }

        
        ThrowEnemyAI throwEnemyAI = enemy.GetComponent<ThrowEnemyAI>();
        if (throwEnemyAI != null)
        {
            throwEnemyAI.target = player;
        }
    }
}