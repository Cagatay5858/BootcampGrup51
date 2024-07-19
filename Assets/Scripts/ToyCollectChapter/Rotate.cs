using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour   
{
    public Transform basket; // Sepet referans�

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

        float moveDuration = 1.0f; // Objenin sepete ula�ma s�resi
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            toy.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            toy.transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        toy.transform.SetParent(basket); // Objeyi sepete �ocuk yap
        toy.transform.localPosition = Vector3.zero; // Objeyi sepetin merkezine konumland�r
        toy.transform.localRotation = Quaternion.identity; // Objeyi sepetin rotasyonuna s�f�rla

        // Rigidbody varsa kinematik yaparak fizik etkilerinden ��kar
        Rigidbody rb = toy.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
}
 
    



    





