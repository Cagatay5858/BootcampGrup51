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
    public bool isBedConstructed = false;
    public bool isPlayerSleeping = false;

    public ParticleSystem craftingParticleEffect;
    public GameObject largeColliderObject;
    public PopupUI popupUI;
    public Animator playerAnimator; // Reference to the player's Animator component

    public Transform sleepPosition; // Position where the player should be repositioned

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
        playerAnimator =
            GameObject.FindWithTag("Player").GetComponent<Animator>(); // Assuming the player has a "Player" tag
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerColliding)
        {
            if (!isBedConstructed)
            {
                if (playerInventory.stickCount >= requiredStickCount &&
                    playerInventory.plantCount >= requiredPlantCount)
                {
                    ConstructBed();
                }
                else
                {
                    popupUI.ShowPopup(playerInventory.stickCount, requiredStickCount, playerInventory.plantCount,
                        requiredPlantCount);
                }
            }
            else if (!isPlayerSleeping)
            {
                PlayerSleep();
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

    void PlayerSleep()
    {
        isPlayerSleeping = true;
        RepositionPlayer();
        playerAnimator.SetTrigger("Sleep"); 
    }

    void RepositionPlayer()
    {
        Transform playerTransform = playerAnimator.transform; 
        CharacterController controller = playerTransform.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }
        playerTransform.position = sleepPosition.position; 
        playerTransform.rotation = sleepPosition.rotation; 
        if (controller != null)
        {
            controller.enabled = true;
        }

    }
}