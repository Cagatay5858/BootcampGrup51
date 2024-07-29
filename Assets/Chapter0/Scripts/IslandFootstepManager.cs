using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public AudioClip[] footstepSounds; 
    public float minTimeBetweenFootsteps = 0.3f; 
    public float maxTimeBetweenFootsteps = 0.6f; 

    private AudioSource audioSource; 
    private bool isWalking = false; 
    private float timeSinceLastFootstep;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
    }
    private void Update()
    {
       
        if (isWalking)
        {
         
            if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
            {
                
                AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
                
               
                if (footstepSound != null)
                {
                    audioSource.PlayOneShot(footstepSound);
                    timeSinceLastFootstep = Time.time;
                }
              
            }
        }
    }
    
    public void StartWalking()
    {
        isWalking = true;
    }
    
    public void StopWalking()
    {
        isWalking = false;
    }
    
    public bool IsWalking()
    {
        return isWalking;
    }
}