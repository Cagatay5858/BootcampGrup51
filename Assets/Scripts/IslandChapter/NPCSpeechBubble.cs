using UnityEngine;
using TMPro;

public class NPCSpeechBubble : MonoBehaviour
{
    public GameObject speechBubblePrefab;
    private GameObject speechBubbleInstance;
    private Transform headTransform;

    private Canvas canvas; 
    private Camera mainCamera;

    void Start()
    {
        headTransform = transform.Find("Head"); 
        canvas = FindObjectOfType<Canvas>();
        mainCamera = Camera.main;
       
    }

    public void ShowSpeechBubble(string message)
    {
        
        if (speechBubbleInstance == null)
        {
            speechBubbleInstance = Instantiate(speechBubblePrefab, canvas.transform);
        }

        TMP_Text speechText = speechBubbleInstance.GetComponentInChildren<TMP_Text>();
        if (speechText != null)
        {
            speechText.text = message;
        }
        
        speechBubbleInstance.SetActive(true);
        UpdateSpeechBubblePosition();
    }

    public void HideSpeechBubble()
    {
        if (speechBubbleInstance != null)
        {
            speechBubbleInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (speechBubbleInstance != null && speechBubbleInstance.activeSelf)
        {
            UpdateSpeechBubblePosition();
        }
    }

    void UpdateSpeechBubblePosition()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(headTransform.position);
        speechBubbleInstance.transform.position = screenPos;
    }
}