
using UnityEngine;

public class ChaseSpeedBoost : ChasePowerUp
{
    public float speedMultiplier = 2.0f;

    public override void ApplyEffect(TeddyBearChaseController player)
    {
        player.StartCoroutine(player.SpeedBoostCoroutine(duration, speedMultiplier));
    }
}