
using UnityEngine;

public abstract class ChasePowerUp : MonoBehaviour
{
    public float duration = 5.0f;
    public abstract void ApplyEffect(TeddyBearChaseController player);
}