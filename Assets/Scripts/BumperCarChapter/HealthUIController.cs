using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HealthUIController : MonoBehaviour
{
    public GameObject healthPanelPrefab; // Prefab for the health display container
    public Transform healthPanelContainer; // The parent transform for health displays
    public GameObject healthDisplayItemPrefab; // Prefab for each health display item

    private Dictionary<GameObject, HealthDisplay> healthDisplays = new Dictionary<GameObject, HealthDisplay>();

    void Start()
    {
        // Instantiate the health panel prefab
        if (healthPanelPrefab != null && healthPanelContainer != null)
        {
            Instantiate(healthPanelPrefab, healthPanelContainer);
        }

        // Find all cars with the Health component and add their health displays
        var allCars = FindObjectsOfType<EnemyCarController>();
        int index = 1;
        foreach (var car in allCars)
        {
            AddHealthDisplay(car.gameObject, index++);
        }

        // Add the player's car health display
        var playerCar = FindObjectOfType<BumperCarController>();
        if (playerCar != null)
        {
            AddHealthDisplay(playerCar.gameObject, index++);
        }
    }

    void AddHealthDisplay(GameObject car, int index)
    {
        if (healthDisplayItemPrefab != null && healthPanelContainer != null)
        {
            var healthDisplayItem = Instantiate(healthDisplayItemPrefab, healthPanelContainer).GetComponent<HealthDisplay>();
            healthDisplayItem.Initialize(car, index);
            healthDisplays[car] = healthDisplayItem;
        }
    }

    public void UpdateHealth(GameObject car, int currentHealth, int maxHealth)
    {
        if (healthDisplays.ContainsKey(car))
        {
            healthDisplays[car].UpdateHealth(currentHealth, maxHealth);
        }
    }
}