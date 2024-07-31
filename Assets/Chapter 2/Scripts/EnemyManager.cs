using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyCars; // Referans olarak düşman arabaları
    private int enemiesDestroyed = 0;

    void Start()
    {
        // Tüm düşman arabalarına event listener ekleyin
        foreach (GameObject enemy in enemyCars)
        {
            enemy.GetComponent<EnemyCarController>().OnEnemyDestroyed += HandleEnemyDestroyed;
        }
    }

    private void HandleEnemyDestroyed()
    {
        enemiesDestroyed++;
        if (enemiesDestroyed >= 3)
        {
            // 3 düşman yok edildiğinde Chapter'ı tamamla ve adaya dön
            SceneManager.Instance.CompleteChapter();
        }
    }
}