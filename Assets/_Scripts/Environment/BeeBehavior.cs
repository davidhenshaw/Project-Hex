using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeeBehavior : MonoBehaviour
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

    [SerializeField]
    GameObject pollenParticles;

    [SerializeField]
    FlowerType pollenType;

    public BeeBehavior leaderBee;
    public BeeBehavior followerBee;
    MovementController _movementController;

    Sequence _landingSequence;
    public float landAmount = -0.3f;
    public float landingDuration = 0.7f;
    public FlowerType PollenType { get => pollenType; }

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
        var beeSprite = GetComponentInChildren<BeelineSprite>();

        //Set up DOTween sequences
        _landingSequence = DOTween.Sequence();
        _landingSequence.Append(beeSprite.transform.DOScale(0.5f, landingDuration/2));
        _landingSequence.Insert(0, beeSprite.transform.DOLocalMoveY(landAmount, landingDuration));
        _landingSequence.SetLoops(2, LoopType.Yoyo);
        _landingSequence.Rewind();

        var _hoverSequence = DOTween.Sequence();
        _hoverSequence.Append(beeSprite.transform.DOLocalMoveY(-0.2f, 0.5f));
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
        BoardElement[] overlappingObjects = GetOverlappingObjects();

        if (overlappingObjects == null)
            return;

        if (followerBee)
            followerBee.TriggerInteract();

        _landingSequence.Play().OnComplete(()=> 
        { 
            _landingSequence.Rewind(); 
        });


        foreach (BoardElement obj in overlappingObjects)
        {
            if(obj.TryGetComponent(out IInteractive interactable))
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
        IsPollenated = true;

        pollenParticles = Instantiate(type.ParticlesPrefab, gameObject.transform);
    }
    
    [ContextMenu("Kill Bee")]
    public void Kill()
    {
        AudioManager.PlayOneShot(AudioManager.Instance.beeDied);

        GetBeelineController().RemoveBee(this);
    }

    public void SetLeader(BeeBehavior otherBee)
    {
        leaderBee = otherBee;

        if (TryGetComponent(out BoardFollower followerComponent))
        {
            followerComponent.Leader = otherBee.GetComponent<BoardElement>();
        }
    }

    public void SetFollower(BeeBehavior otherBee)
    {
        followerBee = otherBee;

        if (otherBee.TryGetComponent(out BoardFollower followerComponent))
        {
            followerComponent.SetToFollow(this.GetComponent<BoardElement>());
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

    BoardElement[] GetOverlappingObjects()
    {
        var myBoardElement = GetComponentInParent<BoardElement>();

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
