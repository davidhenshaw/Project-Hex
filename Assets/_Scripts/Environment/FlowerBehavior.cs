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
            if (!this.IsPollenated)
                return;

            if(bee.IsPollenated)
            {
                CrossBreed(bee.PollenType);
            }
            else
            {
                bee.SetPollen(type);
                this.ClearPollen();
            }
        }
    }

    public void ClearPollen()
    {
        pollenParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        IsPollenated = false;
    }

    public void CrossBreed(FlowerType other)
    {
        GameObject offspring;
        FlowerManager.Instance.TryGetCrossbreed(this.type, other, out offspring);

        Instantiate(offspring, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
