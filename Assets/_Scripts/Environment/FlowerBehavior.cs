using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public interface IInteractive
{
    void OnInteract(GameObject caller);
}

public class FlowerBehavior : GridEntity, IInteractive
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

    [SerializeField]
    Color fadeColor;

    SpriteRenderer[] childSprites;

    private void Awake()
    {
        pollenParticles = GetComponentInChildren<ParticleSystem>();
        childSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    public void OnInteract(GameObject caller)
    {
        if(caller.TryGetComponent(out BeeBehavior bee))
        {
            TryPollenate(bee);
        }
    }

    void PlayFlowerWiggle()
    {
        float punchAmount = 1;

        foreach(SpriteRenderer sprite in childSprites)
        {
            sprite.transform.DOPunchScale(new Vector3(0, punchAmount, 0), 0.3f);
        }
    }

    void FadeFlowerColor()
    {
        foreach (SpriteRenderer sprite in childSprites)
        {
            sprite.DOColor(fadeColor, 0.3f);
        }
    }

    void TryPollenate(BeeBehavior bee)
    {
        if (!this.HasPollen)
            return;

        if (bee.IsPollenated)
        {
            if (!TryCrossBreed(bee.PollenType))
                return;
            bee.ClearPollen();
            AudioManager.PlayOneShot(AudioManager.Instance.flowerPollenated);
        }
        else
        {
            bee.SetPollen(type);
            PlayFlowerWiggle();
            //FadeFlowerColor();
            //this.ClearPollen();
        }
    }

    public void ClearPollen()
    {
        pollenParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        HasPollen = false;
    }

    public bool TryCrossBreed(FlowerType other)
    {
        GameObject offspring;

        if (!FlowerManager.Instance.TryGetCrossbreed(this.type, other, out offspring))
            return false;

        Instantiate(offspring, transform.position, transform.rotation, Board.grid.transform);
        Instantiate(pollenBurstPrefab, transform.position, transform.rotation, Board.grid.transform);

        var offspringType = offspring.GetComponent<FlowerBehavior>().type;
        flowerCrossbred?.Invoke(offspringType);
        
        Destroy(gameObject);

        return true;
    }
}
