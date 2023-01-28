using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="FlowerType")]
public class FlowerType : ScriptableObject
{
    [SerializeField]
    string displayName;

    [SerializeField]
    GameObject particlePrefab;

    [SerializeField]
    Sprite icon;

    [SerializeField]
    bool canPollinate;

    public string DisplayName { get => displayName;  }
    public GameObject ParticlesPrefab { get => particlePrefab; }
    public Sprite Icon { get => icon; }
    public bool CanPollinate { get => canPollinate; }
}
