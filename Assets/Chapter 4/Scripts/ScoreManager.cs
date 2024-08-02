using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int currentScore = 0;
    public int targetScore = 400;
    public TMP_Text scoreText;

    public delegate void TargetScoreReached();

    public event TargetScoreReached OnTargetScoreReached;

    private bool targetScoreReached = false;

    private void Update()
    {
        CheckTargetScore();
    }

    private void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
        CheckTargetScore();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    void CheckTargetScore()
    {
        if (currentScore >= targetScore && !targetScoreReached)
        {
            targetScoreReached = true;
            if (OnTargetScoreReached != null)
            {
                Debug.Log("Target score reached! Invoking OnTargetScoreReached.");
                OnTargetScoreReached.Invoke();
            }
            else
            {
                Debug.LogWarning("OnTargetScoreReached is null.");
            }
        }
    }
}