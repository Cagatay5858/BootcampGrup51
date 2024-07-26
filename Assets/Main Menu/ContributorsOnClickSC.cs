using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContributorsOnClickSC : MonoBehaviour
{
    public void RevealContributors()
    {
        //options.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    public void hideContributors()
    {
        gameObject.SetActive(false);
    }
}
