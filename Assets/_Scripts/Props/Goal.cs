using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Goal : GridEntity
{
    public bool IsOpen = false;

    [SerializeField]
    LayerMask unlockedLayer;
    [SerializeField]
    Sprite lockIcon;
    [SerializeField]
    Sprite unlockIcon;

    UIMarkerWidget _uiLockMarker;

    [Space]

    public List<FlowerCount> flowerRequirements;
    
    [Min(1)]
    public int RequiredBees;
    
    public int CurrentBees
    {
        get;
        private set;
    }

    public Dictionary<FlowerType, int> CurrentFlowers = new Dictionary<FlowerType, int>();

    private void Awake()
    {
        GameEvents.Instance.GameplayUIInit.AddListener(OnGameplayUIInit);
        FlowerBehavior.flowerCrossbred += OnFlowerCrossbred;
    }

    protected override void Start()
    {
        base.Start();

        GameEvents.Instance.GoalInit?.Invoke(flowerRequirements.ToArray());
        GameEvents.Instance.BeeProgressUpdated?.Invoke(CurrentBees, RequiredBees);

    }

    private void OnDisable()
    {
        FlowerBehavior.flowerCrossbred -= OnFlowerCrossbred;
    }

    void OnGameplayUIInit(GameplayUIScreen gameplayUI)
    {
        _uiLockMarker = gameplayUI.CreateUIMarker(transform, lockIcon);
    }

    void OnFlowerCrossbred(FlowerType type)
    {
        //If the key exists already, nothing bad happens
        CurrentFlowers.TryAdd(type, 0);

        CurrentFlowers[type] += 1;

        

        if (CheckRequiredFlowers())
        {
            GameEvents.Instance.FlowersReached?.Invoke();
            UnlockGoal();

            AudioManager.PlayOneShot(AudioManager.Instance.allFlowersPollenated);
        }
    }

    bool CheckRequiredFlowers()
    {
        foreach(FlowerCount reqFlower in flowerRequirements)
        {
            FlowerType type = reqFlower.type;
            int reqCount = reqFlower.count;
            int currentCount;

            //This flower type might not exist in the current flowers dict yet if it hasn't been crossbred yet
            if(CurrentFlowers.TryGetValue(type, out currentCount))
            {
                GameEvents.Instance.FlowerProgressUpdated?.Invoke(type, currentCount, reqCount);

                if (currentCount >= reqCount)
                    continue;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    void OnBeeEntered(BeeBehavior bee)
    {
        CurrentBees++;
        Debug.Log("bee entered goal");
        GameEvents.Instance.BeeProgressUpdated?.Invoke(CurrentBees, RequiredBees);

        if(bee.IsFirst)
        {
            StartCoroutine(AbsorbFollowerBees(bee));
        }
        
        if(CurrentBees == RequiredBees)
        {
            GameEvents.Instance.GoalReached?.Invoke();
            AudioManager.PlayOneShot(AudioManager.Instance.levelComplete);
            Debug.Log("Congratulations! Level Complete.");
        }
    }

    [ContextMenu("Unlock")]
    void UnlockGoal()
    {
        IsOpen = true;

        gameObject.layer = unlockedLayer;

        StartCoroutine(OnGoalUnlock());
        Debug.Log("Goal Unlocked");
    }

    IEnumerator OnGoalUnlock()
    {
        _uiLockMarker.SetIcon(unlockIcon);
        _uiLockMarker.transform.DOShakeRotation(0.5f);

        yield return new WaitForSeconds(0.8f);

        Destroy(_uiLockMarker.gameObject);
    }

    IEnumerator AbsorbFollowerBees(BeeBehavior head)
    {
        BeeBehavior currBee = head.followerBee;

        yield return new WaitForSeconds(0.2f);
        head.gameObject.SetActive(false); //assumes head be is already on the goal tile

        while(currBee)
        {
            GridEntityMovement mover = currBee.GetComponent<GridEntityMovement>();
            mover.Move(GridPosition);

            yield return new WaitForSeconds(0.2f);

            currBee.gameObject.SetActive(false);
            currBee = currBee.followerBee;
            
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnTileEnter(GridEntity other)
    {
        if(other.TryGetComponent(out BeeBehavior bee))
        {
            OnBeeEntered(bee);
        }
    }
}

[System.Serializable]
public struct FlowerCount
{
    public FlowerType type;
    public int count;

    public FlowerCount(FlowerType type, int count)
    {
        this.count = count;
        this.type = type;
    }
}
