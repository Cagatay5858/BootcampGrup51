using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class PickUpObject : MonoBehaviour
{
    public GameObject heldObject;
    public float radius = 2f;
    public float distance = 2f;
    public float height = 1f;

    private void Update()
    {

        var t = transform;
        var pressedE = Input.GetButtonDown("PickUp");

        if (heldObject)
        {

            if (pressedE)
            {
                var rigidbody = heldObject.GetComponent<Rigidbody>();
                rigidbody.drag = 1f;
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                heldObject = null;
                Debug.Log("E tuþuna basýldý");
            }
        }
        else
        {
            if (pressedE)
            {


                var hits = Physics.SphereCastAll(t.position, radius, t.forward, radius);
                var hitIndex = Array.FindIndex(hits, hit => hit.transform.tag == "Pickupable");


                if (hitIndex != -1)
                {
                    var hitObject = hits[hitIndex].transform.gameObject;
                    heldObject = hitObject;
                    var rigidbody = heldObject?.GetComponent<Rigidbody>();
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    rigidbody.drag = 25f;
                    rigidbody.useGravity = false;

                }

            }
        }
    }
    private void FixedUpdate()
    {
        var t = transform;
        var rigidbody = heldObject.GetComponent<Rigidbody> ();
        var moveTo = t.position + distance * t.forward + height * t.up;
        var difference = moveTo - heldObject.transform.position;
        rigidbody.AddForce(difference * 500);
        heldObject.transform.position = moveTo;
    }
}















            












































       
      

 

