using System.Collections;
using UnityEngine;

public class GearCollect : MonoBehaviour
{
    public void Collect()
    {
       
        if (GameController.Instance != null)
        {
           
            GameController.Instance.LoadChapter(chapterNumber:2); 
        }
    }
}