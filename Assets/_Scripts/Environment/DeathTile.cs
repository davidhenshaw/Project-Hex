using System;
using UnityEngine;

public class DeathTile : BoardElement
{
    public static event Action GoalReached;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IDeathTileInteractable player = collider.GetComponent<IDeathTileInteractable>();
        if (player == null)
            return;

        player.Interact();
    }
}

public interface IDeathTileInteractable
{
    void Interact();
}
