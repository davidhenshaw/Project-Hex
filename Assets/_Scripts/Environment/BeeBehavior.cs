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

    public bool IsLeader
    {
        get { return (!headBee && GetComponent<PlayerController>()); }
    }

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
        Destroy(gameObject);
    }

    public void RemoveFromBeeline(bool updateNeighbors)
    {
        if (headBee)
        {
            headBee.followerBee = null;
            if(updateNeighbors)
                headBee.OnBeelineUpdated();
        }

        if (followerBee)
        {
            followerBee.headBee = null;
            if (updateNeighbors)
                followerBee.OnBeelineUpdated();
        }
    }

    public void OnBeelineUpdated()
    {
        if(headBee)
        {
                //This object is now a follower bee
            if (TryGetComponent(out PlayerController controller))
            {
                Destroy(controller);
                var follower = gameObject.AddComponent<BoardFollower>();
                var leader = headBee.GetComponent<ElementMovement>();

                follower.toFollow = leader;
            }
            return;
        }       
        else 
        {
                //This object is now a leader bee
            if(!TryGetComponent(out PlayerController controller))
            {
                gameObject.AddComponent<PlayerController>();
            }

            if(TryGetComponent(out BoardFollower follower))
            {
                Destroy(follower);
            }
            return;
        }
    }

    private void OnDisable()
    {
        RemoveFromBeeline(false);
    }

    private void OnDestroy()
    {
        RemoveFromBeeline(true);
    }

    BoardElement[] GetOverlappingObjects()
    {
        var myBoardElement = GetComponentInParent<BoardElement>();
        var overlappingObjects = myBoardElement.GetOverlappingObjects();

        return overlappingObjects;
    }
}
