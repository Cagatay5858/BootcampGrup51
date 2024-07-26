using System.Collections;
using UnityEngine;

public class CollectibleManager : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        if (gameObject.CompareTag("Stick"))
        {
            StartCoroutine(PlayAnimationAndAddStick());
            return true;
        }
        else if (gameObject.CompareTag("Plant"))
        {
            StartCoroutine(PlayAnimationAndAddPlant());
            return true;
        }
        return false;
    }

    public ThirdPersonController thirdPersonController;
    private Animator teddyBearAnimator;
    public Inventory Inventory;
    public bool inReach;
    public GameObject stick;
    public GameObject plant;
    public GameObject TeddyBear;

    private void Start()
    {
        inReach = false;

        if (TeddyBear != null)
        {
            teddyBearAnimator = TeddyBear.GetComponent<Animator>();
        }
        if (Inventory == null)
        {
            Inventory = FindObjectOfType<Inventory>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FootCollider"))
        {
            inReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FootCollider"))
        {
            inReach = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inReach && teddyBearAnimator != null)
        {
            if (gameObject.CompareTag("Stick"))
            {
                StartCoroutine(PlayAnimationAndAddStick());
            }
            else if (gameObject.CompareTag("Plant"))
            {
                StartCoroutine(PlayAnimationAndAddPlant());
            }
        }
    }

    private IEnumerator PlayAnimationAndAddStick()
    {
        teddyBearAnimator.SetBool("isLifting", true);
        yield return new WaitForSeconds(0.7f);
        Inventory.AddStick();
        Destroy(gameObject);
        teddyBearAnimator.SetBool("isLifting", false);
    }

    private IEnumerator PlayAnimationAndAddPlant()
    {
        teddyBearAnimator.SetBool("isLifting", true);
        yield return new WaitForSeconds(0.7f);
        Inventory.AddPlant();
        Destroy(gameObject);
        teddyBearAnimator.SetBool("isLifting", false);
    }
}