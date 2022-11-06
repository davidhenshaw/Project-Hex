using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBehavior : GridEntity, IInteractive
{
    public void OnInteract(GameObject caller)
    {
        if (caller.TryGetComponent(out BeeBehavior bee))
        {
            bee.Kill();
        }
    }

}
