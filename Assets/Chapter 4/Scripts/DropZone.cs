using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
   public int scoreIncrement = 10;
   private ScoreManager scoreManager;

   private void Start()
   {
      scoreManager = FindObjectOfType<ScoreManager>();
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Toy"))
      {
         scoreManager.AddScore(scoreIncrement);
      }
   }
}
