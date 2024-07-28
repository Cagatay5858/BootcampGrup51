using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 7f;
        


        public float damageAmount = 10f;

        private void OnTriggerEnter(Collider other) {
            HealthSystemComponent healthSystemComponent = other.GetComponent<HealthSystemComponent>();
            if (healthSystemComponent != null) {
                Debug.Log("zttirizortzort");
                healthSystemComponent.GetHealthSystem().Damage(damageAmount);
                Destroy(gameObject); 
            }
        }
    


    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
}

