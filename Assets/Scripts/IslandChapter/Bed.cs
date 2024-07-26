using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
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
    public bool isBedConstructed = false; 

    public ParticleSystem craftingParticleEffect;
    public GameObject largeColliderObject;
    public PopupUI popupUI;

    private Animator playerAnimator;
    private Transform playerTransform;
    private ThirdPersonController playerController;

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
        playerAnimator = FindObjectOfType<Animator>();
        playerController = FindObjectOfType<ThirdPersonController>();

        largeColliderObject.GetComponent<Collider>().isTrigger = true;
        popupUI = FindObjectOfType<PopupUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerColliding)
        {
            if (!isBedConstructed)
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
            else
            {
                TeleportPlayerToBed();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerColliding = true;
            interactor._interactionPromptUI.SetUp(transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerColliding = false;
            interactor._interactionPromptUI.Close();
        }
    }

    void ConstructBed()
    {
        isBedConstructed = true; 
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
        if (craftingParticleEffect != null && !craftingParticleEffect.isPlaying)
        {
            craftingParticleEffect.Play();
        }
    }

    void TeleportPlayerToBed()
    {
        playerController.enabled = false;
        playerTransform = playerController.transform;
        playerTransform.position = transform.position + new Vector3(0, 1, 0); 
        playerAnimator.SetTrigger("Sleep"); 
        playerController.enabled = true;
    }

    public string InteractionPrompt => "Press E to construct or sleep";

    public bool Interact(Interactor interactor)
    {
        if (!isBedConstructed)
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
        else
        {
            TeleportPlayerToBed();
        }
        return true;
    }
}