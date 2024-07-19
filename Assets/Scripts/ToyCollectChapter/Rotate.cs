using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour   
{
    public Transform basket; // Sepet referansý

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("toy"))
        {
            CollectToy(other.gameObject);
        }
    }

    private void CollectToy(GameObject toy)
    {
        StartCoroutine(MoveToBasket(toy));
    }

    private IEnumerator MoveToBasket(GameObject toy)
    {
        Vector3 originalPosition = toy.transform.position;
        Quaternion originalRotation = toy.transform.rotation;
        Vector3 targetPosition = basket.position;
        Quaternion targetRotation = basket.rotation;

        float moveDuration = 1.0f; // Objenin sepete ulaþma süresi
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            toy.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            toy.transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toy.transform.SetParent(basket); // Objeyi sepete çocuk yap
        toy.transform.localPosition = Vector3.zero; // Objeyi sepetin merkezine konumlandýr
        toy.transform.localRotation = Quaternion.identity; // Objeyi sepetin rotasyonuna sýfýrla

        // Rigidbody varsa kinematik yaparak fizik etkilerinden çýkar
        Rigidbody rb = toy.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
}
 
    



    





