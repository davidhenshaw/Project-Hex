using UnityEngine;

public interface IInteractable
{
    ActionBase[] OnInteract(GameObject caller);
}

public interface IInteractive
{
    ActionBase[] TriggerInteract();
}
