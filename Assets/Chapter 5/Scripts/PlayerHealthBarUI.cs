using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour {
    [SerializeField] private GameObject getHealthSystemGameObject;
    [SerializeField] private Slider slider;
    

    private HealthSystem healthSystem;

    private void Start() {
        if (HealthSystem.TryGetHealthSystem(getHealthSystemGameObject, out healthSystem)) {
            SetHealthSystem(healthSystem);
        } 
    }

    public void SetHealthSystem(HealthSystem healthSystem) {
        if (this.healthSystem != null) {
            this.healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        }
        this.healthSystem = healthSystem;

        UpdateHealthBar();

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e) {
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        slider.value = healthSystem.GetHealthNormalized();
    }

    private void OnDestroy() {
        if (healthSystem != null) {
            healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        }
    }
}