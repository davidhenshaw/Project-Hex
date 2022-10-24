using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowerRequirementUI : MonoBehaviour
{
    [SerializeField]
    FlowerType type;
    public FlowerType Type { get => type; }

    TMP_Text text;

    private void Awake()
    {
        GameEvents.Instance.GoalInit.AddListener(OnRequirementsInit);
        GameEvents.Instance.FlowerProgressUpdated.AddListener(OnFlowerCrossbred);
    }

    private void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void UpdateValues(int curr, int total)
    {
        text.text = $"{curr}/{total}";
    }

    void OnFlowerCrossbred(FlowerType type, int currentCount, int reqCount)
    {
        if(type == this.type)
            UpdateValues(currentCount, reqCount);
    }

    void OnRequirementsInit(FlowerCount[] flowerRequirements)
    {
        foreach (FlowerCount flowerCount in flowerRequirements)
        {
            if (flowerCount.type == this.type)
            {
                UpdateValues(0, flowerCount.count);
                return;
            }
        }

        Destroy(gameObject);
    }
}
