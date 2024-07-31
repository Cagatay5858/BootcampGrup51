using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText; // Serialized field for TMP_Text component
    enum TimerType { Countdown, Stopwatch }
    [SerializeField] private TimerType _timerType;
    [SerializeField] private float timeToDisplay = 120.0f;

    private bool _isRunning;

    private void OnEnable()
    {
        EventManager.TimerStart += EventManagerOnTimerStart;
        EventManager.TimerStop += EventManagerOnTimerStop;
    }

    private void OnDisable()
    {
        EventManager.TimerStart -= EventManagerOnTimerStart;
        EventManager.TimerStop -= EventManagerOnTimerStop;
    }

    private void EventManagerOnTimerStart()
    {
        _isRunning = true;
        Debug.Log("Timer Started");
    }

    private void EventManagerOnTimerStop()
    {
        _isRunning = false;
        Debug.Log("Timer Stopped");
    }

    private void Start()
    {
        EventManager.OnTimerStart();
    }

    private void Update()
    {
        if (!_isRunning) return;

        if (_timerText == null)
        {
            Debug.LogError("TMP_Text component is null. Please ensure it is properly assigned.");
            return;
        }

        if (_timerType == TimerType.Countdown && timeToDisplay <= 0.0f)
        {
            EventManager.OnTimerStop();
            return;
        }

        timeToDisplay += _timerType == TimerType.Countdown ? -Time.deltaTime : Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);
        _timerText.text = timeSpan.ToString(@"mm\:ss\:ff");
    }
}