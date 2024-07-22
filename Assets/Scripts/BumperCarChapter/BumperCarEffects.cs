using UnityEngine;

public class BumperCarEffects : MonoBehaviour
{
    public ParticleSystem sparkleEffect;
    public ParticleSystem boomEffect;

    void Start()
    {
        if (sparkleEffect != null)
        {
            sparkleEffect.Stop();
        }

        if (boomEffect != null)
        {
            boomEffect.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Trigger sparkle effect on antenna
            if (sparkleEffect != null)
            {
                sparkleEffect.Play();
            }
        }
        else if (collision.gameObject.CompareTag("Car"))
        {
            // Trigger boom effect on collision
            if (boomEffect != null)
            {
                boomEffect.transform.position = collision.contacts[0].point;
                boomEffect.Play();
            }
        }
    }
}