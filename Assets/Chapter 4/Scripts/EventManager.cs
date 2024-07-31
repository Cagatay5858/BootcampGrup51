using System;
using UnityEngine;

public static class EventManager
{
    public static event Action TimerStart;
    public static event Action TimerStop;

    public static void OnTimerStart()
    {
        TimerStart?.Invoke();
    }

    public static void OnTimerStop()
    {
        TimerStop?.Invoke();
    }
}