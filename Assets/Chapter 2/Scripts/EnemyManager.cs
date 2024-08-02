using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyCars; 
    private int enemiesDestroyed = 0;

    void Start()
    {
        
        foreach (GameObject enemy in enemyCars)
        {
            enemy.GetComponent<EnemyCarController>().OnEnemyDestroyed += HandleEnemyDestroyed;
        }
    }

    private void HandleEnemyDestroyed()
    {
        enemiesDestroyed++;
        Debug.Log("Enemy destroyed. Total: " + enemiesDestroyed);
        if (enemiesDestroyed >= 3)
        {
            Debug.Log("3 enemies destroyed. Showing UI panel.");
            GameSceneManager.Instance.CompleteChapterAndShowUIPanel();
        }
    }
    
}