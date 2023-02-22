using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.Serialization;

public class FlowerBehavior : GridEntity, IInteractable
{
    public static Action<FlowerType> FlowerCrossbred;
    public static Action<FlowerType> FlowerRemoved;

    public bool HasPollen
    {
        get;
        private set;
    } = true;
    public FlowerType Type { get => _type; }

    ParticleSystem _pollenParticles;

    [SerializeField]
    [FormerlySerializedAs("pollenBurstPrefab")]
    GameObject _pollenBurstPrefab;

    [SerializeField]
    [FormerlySerializedAs("type")]
    FlowerType _type;

    [SerializeField]
    [FormerlySerializedAs("fadeColor")]
    Color _fadeColor;

    SpriteRenderer[] childSprites;

    private void Awake()
    {
        _pollenParticles = GetComponentInChildren<ParticleSystem>();
        childSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    public ActionBase[] OnInteract(GameObject caller)
    {
        var arr = new List<ActionBase>();
        if(caller.TryGetComponent(out BeeBehavior bee))
        {
            var beeAction = TryPollinate(bee);
            var flowerAction = TryCrossBreed(bee.PollenType);

            if(beeAction != null)
                arr.Add(beeAction);
            
            if(flowerAction != null)
            {
                arr.Add(flowerAction);
                arr.Add(new PollinateAction(bee, null));
            }
        }

        return arr.ToArray();
    }

    void PlayPollenBurstEffect()
    {
        Instantiate(_pollenBurstPrefab, transform.position, transform.rotation, Board.grid.transform);
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
            sprite.DOColor(_fadeColor, 0.3f);
        }
    }

    void OnCrossBreed(FlowerType flowerType)
    {
        PlayPollenBurstEffect();
        FlowerCrossbred?.Invoke(flowerType);
    }

    ActionBase TryPollinate(BeeBehavior bee)
    {
        //if (!this.HasPollen)
        //    return;

        if (!bee.IsPollenated)
        //{
        //    if (!TryCrossBreed(bee.PollenType))
        //        return;
        //    bee.ClearPollen();
        //    AudioManager.PlayOneShot(AudioManager.Instance.flowerPollenated);
        //}
        //else
        {
            PlayFlowerWiggle();
            return new PollinateAction(bee, this.Type);
        }
        return null;
    }

    public void ClearPollen()
    {
        _pollenParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        HasPollen = false;
    }

    public ActionBase TryCrossBreed(FlowerType other)
    {
        GameObject offspring;

        if (other == null)
            return null;

        if (!FlowerManager.Instance.TryGetCrossbreed(this._type, other, out offspring))
            return null;
        var action = new FlowerTransformAction(this, offspring);
        action.OnExecute += OnCrossBreed;
        action.OnUndo += (offspringType) => { FlowerRemoved?.Invoke(offspringType); };

        return action;
    }
}
