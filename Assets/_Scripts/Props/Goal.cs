using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : BoardElement
{
    public bool IsOpen = false;

    [SerializeField]
    LayerMask unlockedLayer;

    [Space]

    public List<FlowerCount> flowerRequirements;

    public int CurrentBees
    {
        get;
        private set;
    }

    public int RequiredBees
    {
        get;
        set;
    }

    public Dictionary<FlowerType, int> CurrentFlowers = new Dictionary<FlowerType, int>();
    public Dictionary<FlowerType, int> RequiredFlowers = new Dictionary<FlowerType, int>();

    protected override void Start()
    {
        base.Start();

        FlowerBehavior.flowerCrossbred += OnFlowerCrossbred;
    }

    private void OnDisable()
    {
        FlowerBehavior.flowerCrossbred -= OnFlowerCrossbred;
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

    void OnBeeEntered()
    {
        CurrentBees++;

        if(CurrentBees >= RequiredBees)
        {
            GameEvents.Instance.GoalReached?.Invoke();
            UnlockGoal();
        }
    }

    [ContextMenu("Unlock")]
    void UnlockGoal()
    {
        IsOpen = true;

        gameObject.layer = unlockedLayer;

        Debug.Log("Goal Unlocked");
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
