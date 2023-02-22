using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeelineManager : Singleton<BeelineManager>
{
    public GameObject EmptyBeelinePrefab;
    public GameObject FollowerPrefab;
    public GameObject LeaderPrefab;


    private void OnEnable()
    {
        GameEvents.Instance.OneFlowerTypeLeft.AddListener( CheckBeesForPollen );
    }

    //private void OnDisable()
    //{
    //    GameEvents.Instance.OneFlowerTypeLeft.RemoveListener(CheckBeesForPollen);
    //}

    void CheckBeesForPollen(FlowerType lastFlower)
    {
        bool canPollinate = false;
        var beelines = FindObjectsOfType<BeelineController>();

        foreach(BeelineController beeline in beelines)
        {
            var bees = GetComponentsInChildren<BeeBehavior>();
            foreach(BeeBehavior bee in bees)
            {
                if(bee.PollenType != lastFlower)
                {
                    canPollinate = true;
                }
            }
        }

        if (!canPollinate)
        {
            GameEvents.Instance.GoalImpossible?.Invoke();
            //Debug.Log("No win condition met. No more cross-pollinations can be done", this);
        }
    }
}
