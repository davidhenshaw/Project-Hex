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
    [Min(1)]
    public int RequiredBees;
    
    public int CurrentBees
    {
        get;
        private set;
    }

    public Dictionary<FlowerType, int> CurrentFlowers = new Dictionary<FlowerType, int>();

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

        Debug.Log("Goal Unlocked");
    }

    IEnumerator AbsorbFollowerBees(BeeBehavior head)
    {
        BeeBehavior currBee = head.followerBee;

        yield return new WaitForSeconds(0.2f);
        head.gameObject.SetActive(false); //assumes head be is already on the goal tile

        while(currBee)
        {
            ElementMovement mover = currBee.GetComponent<ElementMovement>();
            mover.Move(GridPosition);

            yield return new WaitForSeconds(0.2f);

            currBee.gameObject.SetActive(false);
            currBee = currBee.followerBee;
            
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnTileEnter(BoardElement other)
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
