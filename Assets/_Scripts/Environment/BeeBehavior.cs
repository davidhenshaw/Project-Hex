using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BeeBehavior : MonoBehaviour, IInteractive
{
    public bool IsPollenated
    {
        get;
        private set;
    } = false;

    public bool IsFirst
    {
        get { return (!leaderBee && GetComponent<PlayerController>()); }
    }

    public bool IsLast
    {
        get { return (!followerBee); }
    }

    /// <summary>
    /// Flag to determine whether this object will disable itself after it validates its movement and moves
    /// </summary>
    public bool DeferredDisable { get; set; } = false;

    GameObject pollenParticles;
    FlowerType pollenType;

    public BeeBehavior leaderBee;
    public BeeBehavior followerBee;
    MovementController _movementController;
    
    [Space]

    Sequence _landingSequence;
    Sequence _hoverSequence;

    public event Action InteractFinished;

    public float landAmount = -0.3f;
    public float landingDuration = 0.7f;
    public FlowerType PollenType { get => pollenType; }

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
        var beeSprite = GetComponentInChildren<BillboardSprite>();

        //Set up DOTween sequences
        _landingSequence = DOTween.Sequence();
        _landingSequence.Append(beeSprite.transform.DOScale(0.5f, landingDuration/2));
        _landingSequence.Insert(0, beeSprite.transform.DOLocalMoveY(landAmount, landingDuration));
        _landingSequence.SetLoops(2, LoopType.Yoyo);
        _landingSequence.Rewind();

        _hoverSequence = DOTween.Sequence();
        _hoverSequence.Append(beeSprite.transform.DOLocalMoveY(-0.2f, 0.8f));
        _hoverSequence.SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
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
    }

    private void OnMoveBlocked(Vector3Int from, Vector3Int to)
    {
        var destinationObjs = Board.Instance.GetObjectsAtPosition(to);
        if (destinationObjs == null)
            return;

        foreach(GridEntity obj in destinationObjs)
        {
            TryBump(obj);
        }
    }

    bool TryBump(GridEntity obj)
    {
        if (!obj.TryGetComponent(out BeeBehavior otherBee))
            return false;

        if (!otherBee.IsLast)
            return false;

        if (otherBee.GetBeelineController().Equals(this.GetBeelineController()))
            return false;

        StartCoroutine(
            BeelineController.Merge(GetBeelineController(), otherBee.GetBeelineController())
            );

        return false;
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
        StartCoroutine(InteractSequence());
    }

    IEnumerator InteractSequence()
    {
        GridEntity[] overlappingObjects = GetOverlappingObjects();

        if (overlappingObjects == null)
            yield break;

        if (followerBee)
            followerBee.TriggerInteract();

        Debug.Log("Start Interact");
        //Play the interact animation and wait for the descend animation to play
        yield return _landingSequence.Play().WaitForElapsedLoops(1);

        DoInteraction();

        Debug.Log("End Interact");


        //Wait for ascent animation to play
        yield return _landingSequence.WaitForElapsedLoops(1);
        _landingSequence.Rewind();
        InteractFinished?.Invoke();
    }

    IEnumerator DeathSequence()
    {
        //Unparent the sprite so I can screw with it without it being destroyed
        var billboard = GetComponentInChildren<BillboardSprite>();
        var spriteGO = billboard.gameObject;


        billboard.transform.DOKill();
        _landingSequence.Kill();
        _hoverSequence.Kill();

        spriteGO.transform.SetParent(BeelineManager.Instance.transform, true);

        GetBeelineController().RemoveBee(this);

        spriteGO.transform.DORotate(new Vector3(360, 0, 0), 0.1f)
            .SetEase(Ease.Linear)
            .SetLoops(3, LoopType.Incremental)
            .OnComplete(() => {
                Destroy(spriteGO);
            });
        
        Destroy(billboard);
        yield return null;

    }

    public void DoInteraction()
    {
        GridEntity[] overlappingObjects = GetOverlappingObjects();

        foreach (GridEntity obj in overlappingObjects)
        {
            if (obj.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnInteract(gameObject);
            }
        }
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
        if (!type)
            return;
        IsPollenated = true;

        pollenParticles = Instantiate(type.ParticlesPrefab, gameObject.transform);
    }
    
    [ContextMenu("Kill Bee")]
    public void Kill()
    {
        AudioManager.PlayOneShot(AudioManager.Instance.beeDied);

        StartCoroutine(DeathSequence());
    }

    public void SetLeader(BeeBehavior otherBee)
    {
        leaderBee = otherBee;

        if (TryGetComponent(out BoardFollower followerComponent))
        {
            followerComponent.Leader = otherBee.GetComponent<GridEntity>();
        }
    }

    public void SetFollower(BeeBehavior otherBee)
    {
        followerBee = otherBee;

        if (otherBee.TryGetComponent(out BoardFollower followerComponent))
        {
            followerComponent.SetToFollow(this.GetComponent<GridEntity>());
        }
    }

    public void RemoveLeader()
    {
        if (TryGetComponent(out BoardFollower followerComponent))
        {
            followerComponent.Leader = null;
        }
        leaderBee = null;
    }

    public void RemoveFollower()
    {
        followerBee = null;
    }

    public BeelineController GetBeelineController()
    {
        return GetComponentInParent<BeelineController>();
    }

    GridEntity[] GetOverlappingObjects()
    {
        var myBoardElement = GetComponentInParent<GridEntity>();

        if (myBoardElement == null)
            return null;

        var overlappingObjects = myBoardElement.GetOverlappingObjects();
        return overlappingObjects;
    }

    public void CopyState(BeeBehavior other)
    {
        other.IsPollenated = true;
        other.pollenParticles = pollenParticles;
        other.SetPollen(pollenType);

        if(pollenParticles)
        {
            Instantiate(pollenParticles, 
                other.transform.position, 
                other.transform.rotation, 
                other.transform);
        }

    }
}
