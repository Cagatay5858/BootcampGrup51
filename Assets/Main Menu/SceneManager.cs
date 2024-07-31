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

    public void StartGame()
    {
        currentChapterIndex = 0;  
        LoadScene(islandScene);
    }

    public void OnLyingDownAnimationComplete()
    {
        
        LoadScene(chapterScenes[currentChapterIndex]);
    }

    public void CollectItem(int itemIndex)
    {
        itemsCollected[itemIndex] = true;
    }

    public void BuildBed()
    {
        isBedBuilt = true;
    }

    public void InteractWithGear()
    {
        // Place collected gear at the trophy place (implementation needed)
        // ...

        LoadScene(islandScene);
    }

    public void CompleteChapter()
    {
        // Adaya dönmek için currentChapterIndex'i güncelle ve adaya geri dön
        currentChapterIndex++;
        if (currentChapterIndex < chapterScenes.Length)
        {
            ReturnToIsland();
        }
        else
        {
            
        }
    }

    public void ReturnToIsland()
    {
        LoadScene(islandScene);
    }

    public void InteractWithNPC()
    {
        
        bed.SetActive(true);
        SetObjectVisibility(bed, true);

        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetActive(true);
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
            
            bed.SetActive(true);
            bed.GetComponent<Renderer>().enabled = isBedBuilt;

            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetActive(!itemsCollected[i]);
            }
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