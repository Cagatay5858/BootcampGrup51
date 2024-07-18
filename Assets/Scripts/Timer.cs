using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;


public class Timer : MonoBehaviour
{
    private TMP_Text _timerText;
    enum TimerType { Countdown, Stopwatch }
    [SerializeField] private TimerType _timerType;
    [SerializeField] private float timeToDisplay = 120.0f;

    private bool _isRunning;

    private void Awake()
    {
        _timerText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        EventManager.TimerStart += EventManagerOnTimerStart;
        EventManager.TimerStop += EventManagerOnTimerStop;
        EventManager.TimerUpdate += EventManagerOnTimerUpdated;
    }

    private void OnDisable()
    {
        EventManager.TimerStart -= EventManagerOnTimerStart;
        EventManager.TimerStop -= EventManagerOnTimerStop;
        EventManager.TimerUpdate -= EventManagerOnTimerUpdated;
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

    private void EventManagerOnTimerUpdated(float value)
    {
        timeToDisplay += value;
        Debug.Log("Timer Updated");
    }

    private void Start()
    {
        EventManager.OnTimerStart();  // Timer'ý oyun baþladýðýnda otomatik olarak çalýþtýr
    }

    private void Update()
    {
        if (!_isRunning) return;

        if (_timerType == TimerType.Countdown && timeToDisplay <= 0.0f)
        {
            EventManager.OnTimerStop();
            return;
        }

        timeToDisplay += _timerType == TimerType.Countdown ? -Time.deltaTime : Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);
        _timerText.text = timeSpan.ToString(@"mm\:ss\:ff");
        Debug.Log("Time: " + _timerText.text);
    }
}





