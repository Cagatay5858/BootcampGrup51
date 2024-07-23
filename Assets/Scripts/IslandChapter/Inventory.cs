using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int stickCount;
    public int plantCount;

    public void AddStick()
    {
        stickCount++;
    }

    public void AddPlant()
    {
        plantCount++;
    }
}