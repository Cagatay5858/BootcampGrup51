using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string powerUpType = "SlowMotion"; 

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered by: " + other.name);
        
        if (other.CompareTag("Car"))
        {
            Debug.Log("PowerUp collected by Car");

            BumperCarController carController = other.GetComponent<BumperCarController>();
            if (carController != null)
            {
                
                if (powerUpType == "SlowMotion")
                {
                    
                    carController.ActivateSlowMotion();
                }
                

                Destroy(gameObject); 
            }
        }
    }
}