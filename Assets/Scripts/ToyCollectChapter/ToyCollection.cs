using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ToyCollection : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private TMP_Text _text;
    private int count = 0;
    private int totalSpawnPoints;
    int y;


  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("toy"))
        {
            count++;
            _text.text = count.ToString();
            other.gameObject.SetActive(false);
          
        }
    }



}
