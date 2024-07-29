using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepClips;
    public float footstepCooldown = 0.3f; 
    private float lastFootstepTime = 0f;

    void Update()
    {
      
    }
   
    public void PlayFootstepSound()
    {
        if (Time.time - lastFootstepTime >= footstepCooldown)
        {
            int index = Random.Range(0, footstepClips.Length);
            footstepAudioSource.clip = footstepClips[index];
            footstepAudioSource.Play();
            lastFootstepTime = Time.time;
        }
    }
}