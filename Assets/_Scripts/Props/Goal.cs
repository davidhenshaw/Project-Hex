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

    public Dictionary<FlowerType, int> FlowerHybridsDict = new Dictionary<FlowerType, int>();
    public Dictionary<FlowerType, int> RegularFlowersDict = new Dictionary<FlowerType, int>();

    private void Awake()
    {
        GameEvents.Instance.GameplayUIInit.AddListener(OnGameplayUIInit);
        FlowerBehavior.FlowerCrossbred += OnFlowerCrossbred;
        FlowerBehavior.FlowerRemoved += OnFlowerRemoved;
    }

    protected override void Start()
    {
        base.Start();

        GameEvents.Instance.GoalInit?.Invoke(flowerRequirements.ToArray());
        GameEvents.Instance.BeeProgressUpdated?.Invoke(CurrentBees, RequiredBees);

        PopulateRegularFlowersDict();
    }

    private void OnDisable()
    {
        FlowerBehavior.FlowerCrossbred -= OnFlowerCrossbred;
    }

    void PopulateRegularFlowersDict()
    {
        var allFlowers = FindObjectsOfType<FlowerBehavior>();
        RegularFlowersDict.Clear();
        foreach (FlowerBehavior flower in allFlowers)
        {
            if (!flower.Type.CanPollinate)
                continue;

            if (RegularFlowersDict.ContainsKey(flower.Type))
                RegularFlowersDict[flower.Type] += 1;
            else
                RegularFlowersDict.TryAdd(flower.Type, 1);
        }
    }

    void OnGameplayUIInit(GameplayUIScreen gameplayUI)
    {
        _uiLockMarker = gameplayUI.CreateUIMarker(transform, lockIcon);
    }

    void OnFlowerRemoved(FlowerType flowerType)
    {
        if(FlowerHybridsDict.TryGetValue(flowerType, out int currentCount))
        {
            int reqCount = 0;
            FlowerHybridsDict[flowerType]--;
            currentCount--;
            foreach (FlowerCount reqFlower in flowerRequirements)
            {
                if (reqFlower.type == flowerType)
                    reqCount = reqFlower.count;
            }

            GameEvents.Instance.FlowerProgressUpdated?.Invoke(flowerType, currentCount, reqCount);
        }

        if(RegularFlowersDict.TryGetValue(flowerType, out int count))
        {
            RegularFlowersDict[flowerType]--;
        }

    }

    void OnFlowerCrossbred(FlowerType type)
    {
        //If the key exists already, nothing bad happens
        FlowerHybridsDict.TryAdd(type, 0);
        FlowerHybridsDict[type] += 1;

        if (CheckRequiredFlowers())
        {
            GameEvents.Instance.FlowersReached?.Invoke();
            UnlockGoal();

            AudioManager.PlayOneShot(AudioManager.Instance.allFlowersPollenated);
        }
        else
        if(IsOneFlowerType(out FlowerType flowerType))
        {
            GameEvents.Instance.OneFlowerTypeLeft?.Invoke(flowerType);
        }
    }

    bool CheckRequiredFlowers()
    {
        foreach(FlowerCount reqFlower in flowerRequirements)
        {
            FlowerType type = reqFlower.type;
            int reqCount = reqFlower.count;
            int currentCount;

            //This flower type might not exist in the current flowers dict if it hasn't been crossbred yet
            if(FlowerHybridsDict.TryGetValue(type, out currentCount))
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

    bool IsOneFlowerType(out FlowerType flowerType)
    {
        flowerType = null;
        PopulateRegularFlowersDict();

        Dictionary<FlowerType, int> tempFlowerList = new Dictionary<FlowerType, int>(RegularFlowersDict);
        foreach(FlowerType flower in RegularFlowersDict.Keys)
        {
            if(RegularFlowersDict[flower] <= 0)
            {
                tempFlowerList.Remove(flower);
            }
        }

        if (tempFlowerList.Count != 1)
            return false;

        foreach(FlowerType flower in tempFlowerList.Keys)
        {//I couldn't find a way to get the keys other than a loop :I
            flowerType = flower;
        }

        return true;
    }

    void OnBeeEntered(BeeBehavior bee)
    {
        CurrentBees++;
        Debug.Log("bee entered goal");
        GameEvents.Instance.BeeProgressUpdated?.Invoke(CurrentBees, RequiredBees);

        if(bee.IsLeader)
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

        _uiLockMarker.gameObject.SetActive(false);
    }

    IEnumerator AbsorbFollowerBees(BeeBehavior head)
    {
        BeeBehavior currBee = head.followerBee;

        yield return new WaitForSeconds(0.2f);
        //head.gameObject.SetActive(false); //assumes head be is already on the goal tile
        _board.TileEnterActions.Add(new DisableEntityAction(head.GetComponent<GridEntity>()));
        while(currBee)
        {
            var entController = currBee.GetComponent<EntityController>();
            //mover.Move(GridPosition);
            _board.TileEnterActions.Add(new MoveAction(entController, GridPosition));
            yield return new WaitForSeconds(0.2f);

            //currBee.gameObject.SetActive(false);
            _board.TileEnterActions.Add(new DisableEntityAction(head.GetComponent<GridEntity>()));
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
