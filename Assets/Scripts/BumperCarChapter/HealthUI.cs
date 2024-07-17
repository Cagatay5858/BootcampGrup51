using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public BumperCarController carController; 
    public Text healthText; 

    void Update()
    {
        if (carController != null && healthText != null)
        {
            healthText.text = "Health: " + carController.currentHealth;
        }
    }
}