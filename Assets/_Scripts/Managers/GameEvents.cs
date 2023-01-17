using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : PersistentSingleton<GameEvents>
{
    public UnityEvent GoalReached;
    public UnityEvent<FlowerCount[]> GoalInit;
    public UnityEvent FlowersReached;
    public UnityEvent AllBeesDied;
    public UnityEvent NextLevelLoadTrigger;

    public UnityEvent<FlowerType, int, int> FlowerProgressUpdated;
    public UnityEvent<FlowerType> OneFlowerTypeLeft;
    public UnityEvent GoalImpossible;
    public UnityEvent<int, int> BeeProgressUpdated;

    public UnityEvent PauseTriggered;
    public UnityEvent UnpauseTriggered;

    public UnityEvent MainMenuLoaded;
    public UnityEvent LevelSceneLoaded;

    public UnityEvent<GameplayUIScreen> GameplayUIInit;

    [ContextMenu("Goal Reached")]
    public void TriggerGoalReached()
    {
        GoalReached?.Invoke();
    }
}
