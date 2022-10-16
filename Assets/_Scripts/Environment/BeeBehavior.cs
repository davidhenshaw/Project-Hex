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
    
    [ContextMenu("Kill Bee")]
    public void Kill()
    {
        gameObject.SetActive(false);
    }

    public void RemoveFromBeeline()
    {
        if(headBee)
        {
            headBee.followerBee = null;
            headBee.OnBeelineUpdated();
        }

        if (followerBee)
        {
            followerBee.headBee = null;
            followerBee.OnBeelineUpdated();
        }

    }

    public void OnBeelineUpdated()
    {
        if(!headBee)
        {
            if(!TryGetComponent(out PlayerController controller))
            {
                //This object is now a leader bee
                gameObject.AddComponent<PlayerController>();
            }
            return;
        }
        else
        {
            if (TryGetComponent(out PlayerController controller))
            {
                Destroy(controller);
                //This object is now a follower bee
                var follower = gameObject.AddComponent<BoardFollower>();
                var leader = headBee.GetComponent<ElementMovement>();

                follower.toFollow = leader;
            }
            return;
        }
    }

    private void OnDisable()
    {
        RemoveFromBeeline();
    }

    BoardElement[] GetOverlappingObjects()
    {
        var myBoardElement = GetComponentInParent<BoardElement>();
        var overlappingObjects = myBoardElement.GetOverlappingObjects();

        return overlappingObjects;
    }
}
