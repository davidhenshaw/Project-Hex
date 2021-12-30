using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Goal : BoardElement
{
    public static event Action GoalReached;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerController player = collider.GetComponent<PlayerController>();
        if (!player)
            return;

        GoalReached.Invoke();
    }
}
