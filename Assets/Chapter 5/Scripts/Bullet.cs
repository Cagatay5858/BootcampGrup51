using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 7f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
}
