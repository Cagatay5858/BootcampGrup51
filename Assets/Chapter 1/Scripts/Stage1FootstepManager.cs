using UnityEngine;

public class Stage1FootstepManager : MonoBehaviour
{
    public AudioClip footstepClip; 
    public float minTimeBetweenFootsteps = 0.3f; 
    public float maxTimeBetweenFootsteps = 0.6f;

    private AudioSource audioSource; 
    private bool isWalking = false; 
    private float timeSinceLastFootstep; 

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = footstepClip;
        audioSource.loop = true; 
    }

    private void Update()
    {
        if (isWalking)
        {
           
            if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
            {
                
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

              
                timeSinceLastFootstep = Time.time;
            }
        }
        else
        {
            
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    public void StartWalking()
    {
        isWalking = true;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopWalking()
    {
        isWalking = false;
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}