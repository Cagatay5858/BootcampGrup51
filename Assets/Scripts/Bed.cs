using UnityEngine;

public class Bed : MonoBehaviour
{
    public Material transparentMaterialMatressPillow;
    public Material normalMaterialMatressPillow;
    public Material transparentMaterialFrame;
    public Material normalMaterialFrame;
    public int requiredStickCount;
    
    private Renderer matressRenderer;
    private Renderer pillowRenderer;
    private Renderer footboardRenderer;
    private Renderer headboardRenderer;
    private Renderer sideboardRenderer;
    private Renderer bedpostsRenderer;
    private Inventory playerInventory;
    private Collider objectCollider;
    public Interactor interactor;
    void Start()
    {
        
        matressRenderer = transform.Find("Bed_Matress").GetComponent<Renderer>();
        pillowRenderer = transform.Find("Bed_Pillow").GetComponent<Renderer>();
        footboardRenderer = transform.Find("Bed_Footboard").GetComponent<Renderer>();
        headboardRenderer = transform.Find("Bed_Headboard").GetComponent<Renderer>();
        sideboardRenderer = transform.Find("Bed_Sideboard").GetComponent<Renderer>();
        bedpostsRenderer = transform.Find("BedPosts_Bed").GetComponent<Renderer>();
        objectCollider = GetComponent<BoxCollider>();
        objectCollider.isTrigger = true;
       
      
        matressRenderer.material = transparentMaterialMatressPillow;
        pillowRenderer.material = transparentMaterialMatressPillow;
        footboardRenderer.material = transparentMaterialFrame;
        headboardRenderer.material = transparentMaterialFrame;
        sideboardRenderer.material = transparentMaterialFrame;
        bedpostsRenderer.material = transparentMaterialFrame;

        playerInventory = FindObjectOfType<Inventory>();
    }
   
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) )
        {
            if ( interactor._numFound == 1 && playerInventory.stickCount >= requiredStickCount)
            {
                ChangeToNormalMaterials();
                objectCollider.isTrigger = false;
            }
        }
    }

    

    void ChangeToNormalMaterials()
    {
        matressRenderer.material = normalMaterialMatressPillow;
        pillowRenderer.material = normalMaterialMatressPillow;
        footboardRenderer.material = normalMaterialFrame;
        headboardRenderer.material = normalMaterialFrame;
        sideboardRenderer.material = normalMaterialFrame;
        bedpostsRenderer.material = normalMaterialFrame;
        
    }
}