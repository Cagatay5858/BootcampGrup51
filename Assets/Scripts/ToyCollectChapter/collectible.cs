using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectible : MonoBehaviour

     
{
    public float swayAmount = 0.1f; // Sallanma miktarý
    public float swaySpeed = 1.0f; // Sallanma hýzý

    private Vector3 _originalPosition;

    private void Start()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.position = _originalPosition + new Vector3(0, sway, 0);
    }
}

    




