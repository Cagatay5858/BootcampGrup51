using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointSetter : MonoBehaviour
{
    private Transform respawnPoint;


    public Transform getRespawnpoint()
    {
        return respawnPoint;
    }

    public void setRespawnpoint(Transform respawnPoint)
    {
        this.respawnPoint = respawnPoint;
    }
}
