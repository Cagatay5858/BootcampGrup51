using System;
using UnityEngine;

public class KillManager : MonoBehaviour
{
    public static KillManager Instance;
    public event Action OnTargetScoreReached;

    private int enemiesKilled = 0;
    public int targetKills = 15;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= targetKills)
        {
            OnTargetScoreReached?.Invoke();
        }
    }

    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }

    public void ResetScore()
    {
        enemiesKilled = 0;
    }
}