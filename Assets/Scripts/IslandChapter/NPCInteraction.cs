using UnityEngine;

public class NPCInteraction : MonoBehaviour


{
    public Animator animator;
    private bool isSpeaking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        isSpeaking = !isSpeaking;
        animator.SetBool("isSpeaking", isSpeaking);
        
    }
}