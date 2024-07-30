using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject gameControllerPrefab;

    public void OnStartButtonPressed()
    {
        // GameController prefabını instantiate et
        if (GameController.Instance == null)
        {
            Instantiate(gameControllerPrefab);
        }
        // Oyunu başlat
        GameController.Instance.StartGame();
    }
}