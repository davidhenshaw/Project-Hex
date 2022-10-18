using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : Singleton<FlowerManager>
{
    [SerializeField]
    List<FlowerCombo> flowerCombos;

    Dictionary<int, GameObject> crossbreedDictionary = new Dictionary<int, GameObject>();

    private void Start()
    {
        BuildFlowerDictionary();
    }

    public bool TryGetCrossbreed(FlowerType parentA, FlowerType parentB, out GameObject offspringPrefab)
    {
        offspringPrefab = crossbreedDictionary[GenerateKey(parentA, parentB)];
        return true;
    }

    void BuildFlowerDictionary()
    {
        foreach(FlowerCombo combo in flowerCombos)
        {
            var parentKey = GenerateKey(combo.flowerA, combo.flowerB);

            if (crossbreedDictionary.ContainsKey(parentKey))
                continue;

            crossbreedDictionary.Add(parentKey, combo.result);
        }
    }

    int GenerateKey(FlowerType flowerA, FlowerType flowerB)
    {
        return flowerA.DisplayName.GetHashCode() + flowerB.DisplayName.GetHashCode();
    }
}
