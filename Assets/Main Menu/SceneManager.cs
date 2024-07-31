using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    public string mainMenuScene = "MainMenu";
    public string loadingScreenScene = "LoadingScreen";
    public string islandScene = "Chapter0";
    public string[] chapterScenes = { "Chapter2", "Chapter3", "Chapter4", "Chapter5" };

    public GameObject bed;
    public List<GameObject> items;

    private int currentChapterIndex = 0;
    private bool isBedBuilt = false;
    private bool[] itemsCollected;

    private string targetScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        itemsCollected = new bool[items.Count];
    }

    void Start()
    {
        LoadGameState();

        KillManager killManager = FindObjectOfType<KillManager>();
        if (killManager != null)
        {
            killManager.OnTargetScoreReached += CompleteChapterAndReturnToIsland;
        }

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.OnTargetScoreReached += CompleteChapterAndReturnToIsland;
        }
    }

    public void StartGame()
    {
        currentChapterIndex = 0;
        LoadScene(islandScene);
    }

    public void OnLyingDownAnimationComplete()
    {
        if (currentChapterIndex < chapterScenes.Length)
        {
            LoadScene(chapterScenes[currentChapterIndex]);
        }
        else
        {
            Debug.LogError("No more chapters available.");
        }
    }

    public void CollectItem(int itemIndex)
    {
        itemsCollected[itemIndex] = true;
        SaveGameState();
    }

    public void BuildBed()
    {
        isBedBuilt = true;
        SaveGameState();
    }

    public void InteractWithGear()
    {
        currentChapterIndex++;
        LoadScene(islandScene);
    }

    public void CompleteChapterAndReturnToIsland()
    {
        if (currentChapterIndex < chapterScenes.Length - 1)
        {
            currentChapterIndex++;
            ReturnToIsland();
        }
        else
        {
            ReturnToIsland();
            Debug.Log("All chapters completed. Game finished.");
        }
    }

    public void CompleteChapter()
    {
        if (currentChapterIndex < chapterScenes.Length - 1)
        {
            currentChapterIndex++;
            ReturnToIsland();
        }
        else
        {
            ReturnToIsland();
            Debug.Log("All chapters completed. Game finished.");
        }
    }

    public void ReturnToIsland()
    {
        if (currentChapterIndex >= chapterScenes.Length)
        {
            Debug.Log("All chapters completed. Returning to island for the last time.");
        }
        else
        {
            LoadScene(islandScene);
        }
    }

    public void InteractWithNPC()
    {
        Debug.Log("Interacted with NPC.");

        bed.SetActive(true);
        Debug.Log("Bed set active.");

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(true);
            Debug.Log($"Item {i} set active.");
        }
    }

    private void LoadScene(string sceneName)
    {
        targetScene = sceneName;
        StartCoroutine(LoadLoadingScreen());
    }

    private IEnumerator LoadLoadingScreen()
    {
        AsyncOperation loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(loadingScreenScene);
        while (!loadingOperation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(3);

        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetScene);
        while (!operation.isDone)
        {
            yield return null;
        }

        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == islandScene)
        {
            bed.SetActive(isBedBuilt);

            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetActive(itemsCollected[i]);
                Debug.Log($"Item {i} active state set to {itemsCollected[i]}.");
            }
        }

        KillManager killManager = FindObjectOfType<KillManager>();
        if (killManager != null)
        {
            killManager.OnTargetScoreReached -= CompleteChapterAndReturnToIsland;
            killManager.OnTargetScoreReached += CompleteChapterAndReturnToIsland;
        }

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.OnTargetScoreReached -= CompleteChapterAndReturnToIsland;
            scoreManager.OnTargetScoreReached += CompleteChapterAndReturnToIsland;
        }
    }

    private void SaveGameState()
    {
        PlayerPrefs.SetInt("CurrentChapterIndex", currentChapterIndex);
        PlayerPrefs.SetInt("IsBedBuilt", isBedBuilt ? 1 : 0);
        for (int i = 0; i < itemsCollected.Length; i++)
        {
            PlayerPrefs.SetInt($"ItemCollected_{i}", itemsCollected[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadGameState()
    {
        currentChapterIndex = PlayerPrefs.GetInt("CurrentChapterIndex", 0);
        isBedBuilt = PlayerPrefs.GetInt("IsBedBuilt", 0) == 1;
        for (int i = 0; i < itemsCollected.Length; i++)
        {
            itemsCollected[i] = PlayerPrefs.GetInt($"ItemCollected_{i}", 0) == 1;
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
}