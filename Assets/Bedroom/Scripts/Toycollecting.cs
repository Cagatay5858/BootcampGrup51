using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toycollecting : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Toys"))
        {
            Destroy(other.gameObject);
        }

    }
}
