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

    public bool IsLast
    {
        get { return (!followerBee); }
    }

    /// <summary>
    /// Flag to determine whether this object will disable itself after it validates its movement and moves
    /// </summary>
    public bool DeferredDisable { get; protected set; } = false;

    [SerializeField]
    GameObject pollenParticles;

    [SerializeField]
    FlowerType pollenType;

    public BeeBehavior headBee;
    public BeeBehavior followerBee;
    MovementController _movementController;


    [Header("Prefabs")]
    [SerializeField]
    GameObject followerPrefab;

    [SerializeField]
    GameObject leaderPrefab;

    public FlowerType PollenType { get => pollenType; }

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
    }

    private void OnEnable()
    {
        if (!_movementController)
            return;

        _movementController.MoveBlocked += OnMoveBlocked;
    }

    private void OnDisable()
    {
        _movementController.MoveBlocked -= OnMoveBlocked;

        RemoveFromBeeline(false);
    }

    private void OnMoveBlocked(Vector3Int from, Vector3Int to)
    {
        var destinationObjs = Board.Instance.GetObjectsAtPosition(to);

        foreach(BoardElement obj in destinationObjs)
        {
            TryBump(obj);
        }
    }

    bool TryBump(BoardElement obj)
    {
        if (!obj.TryGetComponent(out BeeBehavior otherBee))
            return false;

        if (!otherBee.IsLast)
            return false;

        JoinBeeline(otherBee);

        return false;
    }

    void JoinBeeline(BeeBehavior receiverTail)
    {
        if (!receiverTail.IsLast)
            return;

        if (!IsLeader)
            return;

        var newFollowerObj = Instantiate(followerPrefab, transform.position, transform.rotation, Board.Instance.grid.transform);
        var newBeeBehavior = newFollowerObj.GetComponent<BeeBehavior>();

        if(this.followerBee)
        {
            this.followerBee.SetHead(newBeeBehavior);
            newBeeBehavior.SetFollower(this.followerBee);
        }
        receiverTail.SetFollower(newBeeBehavior);

        headBee = null;
        followerBee = null;

        DeferredDisable = true;
    }

    public void OnPostMoveUpdate()
    {
        if(DeferredDisable)
        {
            gameObject.SetActive(false);
        }
    }

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

    public void SetHead(BeeBehavior otherBee)
    {
        headBee = otherBee;

        if (TryGetComponent(out BoardFollower followerComponent))
        {
            followerComponent.toFollow = otherBee.GetComponent<BoardElement>();
        }
    }

    public void SetFollower(BeeBehavior otherBee)
    {
        followerBee = otherBee;

        if(otherBee.TryGetComponent(out BoardFollower followerComponent))
        {
            followerComponent.SetToFollow(this.GetComponent<BoardElement>());
        }
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
