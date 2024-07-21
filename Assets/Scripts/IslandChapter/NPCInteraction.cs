using UnityEngine;

public class NPCInteraction : MonoBehaviour


{
    public Animator animator;
    private bool isSpeaking = false;
    public string[] dialogueLines;
    void Start()
    {
        animator = GetComponent<Animator>();
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
}