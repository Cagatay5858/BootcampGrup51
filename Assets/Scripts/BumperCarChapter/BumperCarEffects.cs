using UnityEngine;

public class BumperCarEffects : MonoBehaviour
{
    public ParticleSystem sparkleEffect;
    public ParticleSystem boomEffect;
    public bool sparkleOnce = true;
    public bool boomOnce = true;


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
            var sparkle = sparkleEffect.emission;
           
            sparkle.enabled = true;

            sparkleEffect.transform.position = collision.contacts[0].point;
            sparkleEffect.Play();

            sparkleOnce = false;
        }
        else if (collision.gameObject.CompareTag("Car"))
        {
         
            var boom = boomEffect.emission;
            boom.enabled = true;
            boomEffect.transform.position = collision.contacts[0].point;
            boomEffect.Play();
            boomOnce = false;
            
        }
    }
}