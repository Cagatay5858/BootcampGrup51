using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    public int stickCount;
    
    public void AddStick()
    {
        stickCount++;
        Debug.Log("Çubuk eklendi! Şu anki çubuk sayısı: " + stickCount);
    }
}