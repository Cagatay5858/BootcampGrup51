using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject gameControllerPrefab;

    public void OnStartButtonPressed()
    {
        SceneManager.Instance.StartGame();
    }
}
