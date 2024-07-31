using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Animator animator;
    private bool isSpeaking = false;
    public string[] dialogueLines;

    public GameObject bed;
    public GameObject[] sticks;
    public GameObject[] plants;

    private InteractionPromptUI interactionPromptUI;

    void Start()
    {
        animator = GetComponent<Animator>();

        SetObjectVisibility(bed, false);
        SetObjectsVisibility(sticks, false);
        SetObjectsVisibility(plants, false);

        interactionPromptUI = FindObjectOfType<InteractionPromptUI>();
    }

    public void Interact()
    {
        isSpeaking = !isSpeaking;
        animator.SetBool("isSpeaking", isSpeaking);
        
        NPCSpeechBubble speechBubble = GetComponent<NPCSpeechBubble>();
        if (speechBubble != null)
        {
            if (isSpeaking)
            {
                speechBubble.ShowSpeechBubble(dialogueLines);
                // Ensure the bed GameObject is active before changing its visibility
                bed.SetActive(true);
                SetObjectVisibility(bed, true);
                SetObjectsVisibility(sticks, true);
                SetObjectsVisibility(plants, true);
                interactionPromptUI.SetUp(null); 
            }
            else
            {
                speechBubble.HideSpeechBubble();
            }
        }
    }

    public void AdvanceDialogue()
    {
        NPCSpeechBubble speechBubble = GetComponent<NPCSpeechBubble>();
        if (speechBubble != null)
        {
            speechBubble.AdvanceDialogue();
        }
    }

    public void StopInteraction()
    {
        isSpeaking = false;
        animator.SetBool("isSpeaking", false);

        NPCSpeechBubble speechBubble = GetComponent<NPCSpeechBubble>();
        if (speechBubble != null)
        {
            speechBubble.HideSpeechBubble();
        }
    }

    private void SetObjectVisibility(GameObject obj, bool isVisible)
    {
        if (obj != null)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = isVisible;
            }
        }
    }

    private void SetObjectsVisibility(GameObject[] objs, bool isVisible)
    {
        foreach (GameObject obj in objs)
        {
            SetObjectVisibility(obj, isVisible);
        }
    }
}