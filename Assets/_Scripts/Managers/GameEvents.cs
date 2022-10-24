using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : PersistentSingleton<GameEvents>
{
    public UnityEvent GoalReached;
    public UnityEvent FlowersReached;
    public UnityEvent AllBeesDied;
    public UnityEvent NextLevelLoadTrigger;

    public UnityEvent PauseTriggered;
    public UnityEvent UnpauseTriggered;

    public bool IsPaused { get; private set; }

    [ContextMenu("Goal Reached")]
    public void TriggerGoalReached()
    {
        GoalReached?.Invoke();
    }

    public void TogglePause()
    {
        if (IsPaused)
            Unpause();
        else
            Pause();
    }

    public void Unpause()
    {
        IsPaused = false;
        UnpauseTriggered?.Invoke();
    }

    public void Pause()
    {
        IsPaused = true;
        PauseTriggered?.Invoke();
    }
}
