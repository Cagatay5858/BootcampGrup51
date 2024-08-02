using System.Collections;
using UnityEngine;

public class GearCollect : MonoBehaviour
{
    public void Collect()
    {
        GameSceneManager.Instance.CompleteChapterAndShowUIPanel();
        
    }
}
