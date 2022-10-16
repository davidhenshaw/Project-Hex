using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBehavior : MonoBehaviour
{
    public bool IsPollenated
    {
        get;
        private set;
    } = false;

    [SerializeField]
    GameObject pollenParticles;

    [SerializeField]
    FlowerType pollenType;

    public BeeBehavior headBee;
    public BeeBehavior followerBee;

    public FlowerType PollenType { get => pollenType; }
    
    public void TriggerInteract()
    {
        BoardElement[] overlappingObjects = GetOverlappingObjects();

        foreach (BoardElement obj in overlappingObjects)
        {
            if(obj.TryGetComponent(out IInteractive interactable))
            {
                interactable.OnInteract(gameObject);
            }
        }

        if (followerBee)
            followerBee.TriggerInteract();
    }

    public void ClearPollen()
    {
        this.pollenType = null;
        IsPollenated = false;

        Destroy(pollenParticles);
    }

    public void SetPollen(FlowerType type)
    {
        this.pollenType = type;
        IsPollenated = true;

        pollenParticles = Instantiate(type.ParticlesPrefab, gameObject.transform);
    }

    BoardElement[] GetOverlappingObjects()
    {
        var myBoardElement = GetComponentInParent<BoardElement>();
        var overlappingObjects = myBoardElement.GetOverlappingObjects();

        return overlappingObjects;
    }
}
