using System;
using UnityEngine;

public class HealthSystem {

    public event EventHandler OnHealthChanged;

    private float health;
    private float healthMax;

    public HealthSystem(float healthMax) {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public float GetHealth() {
        return health;
    }

    public float GetHealthNormalized() {
        return health / healthMax;
    }

    public void Damage(float amount) {
        health -= amount;
        if (health < 0) health = 0;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(float amount) {
        health += amount;
        if (health > healthMax) health = healthMax;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public static bool TryGetHealthSystem(GameObject gameObject, out HealthSystem healthSystem) {
        var healthSystemComponent = gameObject.GetComponent<HealthSystemComponent>();
        if (healthSystemComponent != null) {
            healthSystem = healthSystemComponent.GetHealthSystem();
            return true;
        }
        healthSystem = null;
        return false;
    }
}