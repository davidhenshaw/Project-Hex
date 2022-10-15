using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive
{
    void OnInteract(GameObject caller);
}

public class FlowerBehavior : MonoBehaviour, IInteractive
{
    public bool IsPollenated
    {
        get;
        private set;
    } = true;

    [SerializeField]
    ParticleSystem pollenParticles;


    public void OnInteract(GameObject caller)
    {
        if(caller.TryGetComponent(out BeeBehavior bee))
        {
            bee.SetPollen(true);
            this.SetPollen(false);
        }
    }

    public void SetPollen(bool value)
    {
        if (!IsPollenated && value)//If off then turned on
        {
            pollenParticles.Play();
        }

        if (value == false)
        {
            pollenParticles.Stop();
        }

        IsPollenated = value;
    }
}
