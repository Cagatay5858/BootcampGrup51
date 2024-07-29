using UnityEngine;

public class CarSound : MonoBehaviour
{
    public AudioSource carAudioSource;
    public AudioClip carPassingByClip;
    public float triggerDistance = 20f; // Sesin başlaması için gereken mesafe

    private Transform playerTransform;
    private bool hasStartedPlaying = false;

    void Start()
    {
        carAudioSource.clip = carPassingByClip;
        carAudioSource.spatialBlend = 1.0f; // 3D ses için
        carAudioSource.minDistance = 5f; // Tam duyulma mesafesi
        carAudioSource.maxDistance = 50f; // Tamamen duyulmaz mesafe

        // Player'ı tag ile bul ve transform'unu al
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object with tag 'Player' not found!");
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Sesin başlaması için mesafe kontrolü
        if (distanceToPlayer <= triggerDistance && !hasStartedPlaying)
        {
            carAudioSource.Play();
            hasStartedPlaying = true;
        }

        // Sesin pozisyonunu güncelle
        carAudioSource.transform.position = transform.position;
    }
}