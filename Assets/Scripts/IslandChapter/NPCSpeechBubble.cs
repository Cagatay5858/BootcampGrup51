using UnityEngine;
using TMPro;

public class NPCSpeechBubble : MonoBehaviour
{
    public GameObject speechBubblePrefab;
    private GameObject speechBubbleInstance;
    public Transform headTransform;

    private Canvas canvas;
    private Camera mainCamera;
    private string[] dialogueLines;
    private int currentLineIndex = 0;

    void Start()
    {
        headTransform = transform.Find("Head");
        canvas = FindObjectOfType<Canvas>();
        if (headTransform == null)
        {
            Debug.LogError("Head transform not found!");
            return;
        }


        if (canvas == null)
        {
            Debug.LogError("Canvas not found in the scene!");
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
    }

    public void ShowSpeechBubble(string[] lines)
    {
        if (canvas == null || headTransform == null)
        {
            Debug.LogError("Canvas or HeadTransform is missing, cannot show speech bubble!");
            return;
        }

        if (speechBubbleInstance == null)
        {
            speechBubbleInstance = Instantiate(speechBubblePrefab, canvas.transform);
        }

        dialogueLines = lines;
        currentLineIndex = 0;
        speechBubbleInstance.SetActive(true);
        UpdateSpeechBubbleText();
    }

    public void HideSpeechBubble()
    {
        if (speechBubbleInstance != null)
        {
            speechBubbleInstance.SetActive(false);
        }
    }

    public void AdvanceDialogue()
    {
        if (dialogueLines == null || currentLineIndex >= dialogueLines.Length - 1)
        {
            HideSpeechBubble();
            return;
        }

        currentLineIndex++;
        UpdateSpeechBubbleText();
    }

    void UpdateSpeechBubbleText()
    {
        TMP_Text speechText = speechBubbleInstance.GetComponentInChildren<TMP_Text>();
        if (speechText != null)
        {
            speechText.text = dialogueLines[currentLineIndex];

        }
    }

    void Update()
        {
            if (speechBubbleInstance != null)
            {
                UpdateSpeechBubblePosition();
            }
        }
        
        

        void UpdateSpeechBubblePosition()
        {
            if (mainCamera == null || headTransform == null || speechBubbleInstance == null)
            {
                return;
            }
            Vector3 screenPos = mainCamera.WorldToScreenPoint(headTransform.position);
            speechBubbleInstance.transform.position = screenPos;

        }
    }

