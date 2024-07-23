using UnityEngine;

public class Bed : MonoBehaviour
{
    public Material transparentMaterialMatressPillow;
    public Material normalMaterialMatressPillow;
    public Material transparentMaterialFrame;
    public Material normalMaterialFrame;
    public int requiredStickCount;
    public int requiredPlantCount;

    private Renderer matressRenderer;
    private Renderer pillowRenderer;
    private Renderer footboardRenderer;
    private Renderer headboardRenderer;
    private Renderer sideboardRenderer;
    private Renderer bedpostsRenderer;
    private Inventory playerInventory;
    private Collider objectCollider;
    public Interactor interactor;

    public bool isPlayerColliding = false;

    public ParticleSystem craftingParticleEffect;
    public GameObject largeColliderObject;
    public PopupUI popupUI;

    void Start()
    {
        requiredStickCount = 3;
        requiredPlantCount = 3;
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

        largeColliderObject.GetComponent<Collider>().isTrigger = true;
        popupUI = FindObjectOfType<PopupUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerColliding)
        {
            if (playerInventory.stickCount >= requiredStickCount && playerInventory.plantCount >= requiredPlantCount)
            {
                ConstructBed();
            }
            else
            {
                popupUI.ShowPopup(playerInventory.stickCount, requiredStickCount, playerInventory.plantCount, requiredPlantCount);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerColliding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerColliding = false;
        }
    }

    void ConstructBed()
    {
        ChangeToNormalMaterials();
        objectCollider.isTrigger = false;
        largeColliderObject.SetActive(false); 
        PlayCraftingParticleEffect();
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

    void PlayCraftingParticleEffect()
    {
        if (craftingParticleEffect != null)
        {
            craftingParticleEffect.Play();
        }
    }
}