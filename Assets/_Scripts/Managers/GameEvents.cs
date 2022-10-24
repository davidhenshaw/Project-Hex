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

    public UnityEvent MainMenuLoaded;
    public UnityEvent LevelSceneLoaded;

    [ContextMenu("Goal Reached")]
    public void TriggerGoalReached()
    {
        GoalReached?.Invoke();
    }
}
