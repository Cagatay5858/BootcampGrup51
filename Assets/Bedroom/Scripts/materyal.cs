using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materyal : MonoBehaviour
{
  

    public Material newMaterial; 

    void Start()
    {
        
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = newMaterial;
        }
        else
        {
            SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.material = newMaterial;
            }
            else
            {
                Debug.LogError("Renderer bileþeni bulunamadý!");
            }
        }
    }
}


