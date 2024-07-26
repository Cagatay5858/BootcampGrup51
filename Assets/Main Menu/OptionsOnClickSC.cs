using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OptionsOnClickSC : MonoBehaviour
{
    //public Canvas options;

    public void RevealOptions()
    {
        //options.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    public void hideOptions()
    {
        gameObject.SetActive(false);
    }
}
