using UnityEngine;
using UnityEngine.UI;

public class ChapterStartUI : MonoBehaviour
{
    public GameObject chapterStartCanvas;
    private GameObject chapterStartInstance;
    private bool isInitialized = false; 

    void Start()
    {
        if (!isInitialized) // Eğer script daha önce başlatılmadıysa
        {
            isInitialized = true; // Başlatıldı olarak işaretle
            // Oyunu durdur
            Time.timeScale = 0;
            Debug.Log(gameObject.name + " - Time.timeScale in Start: " + Time.timeScale);

            // Canvas'ı instantiate et ve ekranda göster
            chapterStartInstance = Instantiate(chapterStartCanvas, transform);
            Debug.Log(gameObject.name + " - Chapter start canvas instantiated.");

            // Butona basıldığında çalışacak fonksiyonu butona atayın
            Button startButton = chapterStartInstance.GetComponentInChildren<Button>();
            if (startButton != null)
            {
                startButton.onClick.AddListener(OnStartButtonClicked);
                Debug.Log(gameObject.name + " - Start button listener added.");
            }
            else
            {
                Debug.LogError(gameObject.name + " - Start button not found in the canvas prefab.");
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " - Script is already initialized.");
        }
    }

    void OnStartButtonClicked()
    {
        Debug.Log(gameObject.name + " - Button clicked. Time.timeScale before: " + Time.timeScale);

        Destroy(chapterStartInstance);
        Time.timeScale = 1;
        Debug.Log(gameObject.name + " - Time.timeScale after: " + Time.timeScale);

        
    }

   
}