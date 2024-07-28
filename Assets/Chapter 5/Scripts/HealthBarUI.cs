using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
    [SerializeField] private GameObject getHealthSystemGameObject;
    [SerializeField] private Image image;

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
        image.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void OnDestroy() {
        healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
    }
}