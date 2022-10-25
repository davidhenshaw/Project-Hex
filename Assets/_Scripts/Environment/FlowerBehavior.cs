using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractive
{
    void OnInteract(GameObject caller);
}

public class FlowerBehavior : BoardElement, IInteractive
{
    public static Action<FlowerType> flowerCrossbred;

    public bool HasPollen
    {
        get;
        private set;
    } = true;

    ParticleSystem pollenParticles;
    
    [SerializeField]
    GameObject pollenBurstPrefab;

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
            TryPollenate(bee);
        }
    }

    void TryPollenate(BeeBehavior bee)
    {
        if (!this.HasPollen)
            return;

        if (bee.IsPollenated)
        {
            CrossBreed(bee.PollenType);
            bee.ClearPollen();
            AudioManager.PlayOneShot(AudioManager.Instance.flowerPollenated);
        }
        else
        {
            bee.SetPollen(type);
            this.ClearPollen();
        }
    }

    public void ClearPollen()
    {
        pollenParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        HasPollen = false;
    }

    public void CrossBreed(FlowerType other)
    {
        GameObject offspring;
        FlowerManager.Instance.TryGetCrossbreed(this.type, other, out offspring);

        Instantiate(offspring, transform.position, transform.rotation, Board.grid.transform);
        Instantiate(pollenBurstPrefab, transform.position, transform.rotation, Board.grid.transform);

        var offspringType = offspring.GetComponent<FlowerBehavior>().type;
        flowerCrossbred?.Invoke(offspringType);
        
        Destroy(gameObject);
    }
}
