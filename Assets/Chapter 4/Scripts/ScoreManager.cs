using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;
    public Text scoreText;

    private void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}