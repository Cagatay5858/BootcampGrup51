using UnityEngine;

public class CampfireSoundManager : MonoBehaviour
{
    private AudioSource audioSource; 

    private void Awake()
    {
       
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
         
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger collider
        if (other.CompareTag("Player"))
        {
            // Stop playing the campfire sound
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}