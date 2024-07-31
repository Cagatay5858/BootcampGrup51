using System.Collections;
using UnityEngine;

public class GearCollect : MonoBehaviour
{
    public void Collect()
    {
        SceneManager.Instance.InteractWithGear();
    }
}
