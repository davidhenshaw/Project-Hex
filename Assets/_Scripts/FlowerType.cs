using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="FlowerType")]
public class FlowerType : ScriptableObject
{
    [SerializeField]
    string displayName;

    [SerializeField]
    GameObject particles;

    public string DisplayName { get => displayName;  }
    public ParticleSystem Particles { get => particles.GetComponent<ParticleSystem>(); }


}
