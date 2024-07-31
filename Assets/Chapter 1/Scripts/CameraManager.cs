using UnityEngine;
using Cinemachine;

public class CinemachineCameraManager : MonoBehaviour
{
    public static CinemachineCameraManager Instance;

    public CinemachineFreeLook freeLookCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AssignCameraToPlayer(Transform player)
    {
        if (freeLookCamera != null)
        {
            freeLookCamera.Follow = player;
            freeLookCamera.LookAt = player;
        }
    }
}