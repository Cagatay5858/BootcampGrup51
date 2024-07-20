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
        if (headTransform == null)
        {
            Debug.LogError("Head transform not found!");
            return;
        }

        canvas = FindObjectOfType<Canvas>();
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

    public void ShowSpeechBubble(string message)
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

        TMP_Text speechText = speechBubbleInstance.GetComponentInChildren<TMP_Text>();
        if (speechText != null)
        {
            speechText.text = message;
        }
        else
        {
            Debug.LogError("TMP_Text component not found in speech bubble prefab!");
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
        if (mainCamera == null || headTransform == null)
        {
            return;
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(headTransform.position);
        speechBubbleInstance.transform.position = screenPos;
    }
}
