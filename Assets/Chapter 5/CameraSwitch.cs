using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera aimCamera;
    private ThirdPersonShooterController controller;

    void Start()
    {
        controller = GetComponent<ThirdPersonShooterController>();
    }

    void Update()
    {
        if (controller.animator.GetBool("isAiming"))
        {
            followCamera.Priority = 0;
            aimCamera.Priority = 1;
        }
        else
        {
            followCamera.Priority = 1;
            aimCamera.Priority = 0;
        }
    }
}