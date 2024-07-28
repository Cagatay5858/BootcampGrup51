using UnityEngine;

public class HealthSystemComponent : MonoBehaviour {
    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = new HealthSystem(100f); 
    }

    public HealthSystem GetHealthSystem() {
        return healthSystem;
    }
}