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
        throw new System.NotImplementedException();
    }
}
