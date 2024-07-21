using UnityEngine;
using Cinemachine;

public class BumperCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Transform bumperCar;
    public float mouseSensitivity = 10f;
    public float cameraDistance = 20f;

    private Vector3 offset;

    void Start()
    {
        if (virtualCamera != null && bumperCar != null)
        {
            virtualCamera.Follow = bumperCar;
            virtualCamera.LookAt = bumperCar;

          
            offset = new Vector3(0, cameraDistance, -cameraDistance);
            virtualCamera.transform.position = bumperCar.position + offset;
            virtualCamera.transform.rotation = Quaternion.Euler(45, 0, 0); 
        }
    }

    void Update()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        offset = Quaternion.AngleAxis(mouseX, Vector3.up) * offset;
        offset = Quaternion.AngleAxis(mouseY, Vector3.right) * offset;

        virtualCamera.transform.position = bumperCar.position + offset;
        virtualCamera.transform.LookAt(bumperCar.position);
    }
}