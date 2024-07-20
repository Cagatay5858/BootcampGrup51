
using UnityEngine;

public class ChaseShield : ChasePowerUp
{
    public override void ApplyEffect(TeddyBearChaseController player)
    {
        player.StartCoroutine(player.ShieldCoroutine(duration));
    }
}