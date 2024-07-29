using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GearCollect : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Toplandiginda ara sinematige gececek sinematik sonunda ada bolumune gececek
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
