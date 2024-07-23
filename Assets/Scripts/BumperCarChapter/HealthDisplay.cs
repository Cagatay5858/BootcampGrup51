using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TMP_Text healthText;
    private GameObject car;
    private int carIndex;

    public void Initialize(GameObject car, int index)
    {
        this.car = car;
        this.carIndex = index;
        if (car.GetComponent<EnemyCarController>() != null)
        {
            UpdateHealth(car.GetComponent<EnemyCarController>().currentHealth, car.GetComponent<EnemyCarController>().maxHealth);
        }
        else if (car.GetComponent<BumperCarController>() != null)
        {
            UpdateHealth(car.GetComponent<BumperCarController>().currentHealth, car.GetComponent<BumperCarController>().maxHealth);
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Car {carIndex}: {currentHealth}/{maxHealth}";
        }
    }
}