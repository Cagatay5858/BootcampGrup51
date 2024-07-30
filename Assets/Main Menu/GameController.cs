using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private int currentChapter = 0;
    private IslandState islandState = new IslandState();

    private void Awake()
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
    }

    public void StartGame()
    {
        // Oyunu Chapter0 ile başlat
        LoadChapter(0);
    }

    public void OnLyingDownAnimationComplete()
    {
        // Yatak animasyonu bittiğinde bir sonraki chapter'a geçiş yap
        currentChapter++;
        SaveIslandState();
        StartCoroutine(LoadChapterWithLoadingScreen(currentChapter));
    }

    public void LoadChapter(int chapterNumber)
    {
        // Yükleme ekranını gösterme ve chapter yükleme işlemi
        StartCoroutine(LoadChapterWithLoadingScreen(chapterNumber));
    }

    private IEnumerator LoadChapterWithLoadingScreen(int chapterNumber)
    {
        // Yükleme ekranı sahnesini yükle
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("LoadingScreen");
        yield return loadOperation;

        // 3 saniye bekle
        yield return new WaitForSeconds(3f);

        // Hedef chapter sahnesini yükle
        string chapterSceneName = "Chapter" + chapterNumber;
        SceneManager.LoadScene(chapterSceneName);

        // Hedef sahne yüklendikten sonra ada durumunu geri yükle
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == chapterSceneName);
        LoadIslandState();
    }

    void SaveIslandState()
    {
        PlayerPrefs.SetString("IslandState", JsonUtility.ToJson(islandState));
    }

    void LoadIslandState()
    {
        if (PlayerPrefs.HasKey("IslandState"))
        {
            islandState = JsonUtility.FromJson<IslandState>(PlayerPrefs.GetString("IslandState"));
            RestoreIslandState();
        }
    }

    void RestoreIslandState()
    {
        foreach (string reward in islandState.rewards)
        {
            // Ödülleri adada geri yükleme kodları
        }
    }
}