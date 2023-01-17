using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeeRequirementWidget : MonoBehaviour
{
    [SerializeField]
    TMP_Text beesText;


    private void Awake()
    {
        GameEvents.Instance.BeeProgressUpdated.AddListener(OnBeeNumberUpdated);
    }

    void OnBeeNumberUpdated(int curr, int total)
    {
        beesText.text = $"{curr}/{total}";
    }

}
