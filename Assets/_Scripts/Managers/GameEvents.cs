using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : PersistentSingleton<GameEvents>
{
    public UnityEvent GoalReached;
    public UnityEvent FlowersReached;

    [ContextMenu("Goal Reached")]
    public void TriggerGoalReached()
    {
        GoalReached?.Invoke();
    }
}
