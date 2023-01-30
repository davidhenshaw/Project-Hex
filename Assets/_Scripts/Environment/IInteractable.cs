using UnityEngine;

public interface IInteractable
{
    void OnInteract(GameObject caller);
}

public interface IInteractive
{
    void TriggerInteract();
}
