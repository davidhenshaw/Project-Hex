using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBehavior : GridEntity, IInteractable
{
    public void OnInteract(GameObject caller)
    {
        if (caller.TryGetComponent(out BeeBehavior bee))
        {
            bee.Kill();
        }
    }

    ActionBase[] IInteractable.OnInteract(GameObject caller)
    {
        if (caller.TryGetComponent(out BeeBehavior bee))
        {
            return new ActionBase[] { bee.Kill() };
        }
        else
            return null;

    }
}
