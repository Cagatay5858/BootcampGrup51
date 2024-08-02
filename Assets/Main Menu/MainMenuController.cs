using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject gameControllerPrefab;

    public void OnStartButtonPressed()
    {
        GameSceneManager.Instance.StartGame();
    }
}
