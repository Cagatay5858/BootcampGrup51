using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler1 : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("FinishLine"))
        {
            
            GameSceneManager.Instance.CompleteChapterAndShowUIPanel();
        }
        else if (hit.gameObject.CompareTag("DeactivateCarSpawner"))
        {
            
            DeactivateCarSpawner deactivateScript = hit.gameObject.GetComponent<DeactivateCarSpawner>();
            if (deactivateScript != null)
            {
                deactivateScript.carSpawner.SetActive(false);
            }
        }
    }
}