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

    ParticleSystem pollenParticles;

    [SerializeField]
    FlowerType type;

    private void Awake()
    {
        pollenParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void OnInteract(GameObject caller)
    {
        if(caller.TryGetComponent(out BeeBehavior bee))
        {
            bee.SetPollen(type);
            this.ClearPollen();
        }
    }

    public void ClearPollen()
    {
        pollenParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        IsPollenated = false;
    }

}
