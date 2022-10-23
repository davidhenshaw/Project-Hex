using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Level Info")]
public class LevelInfo : ScriptableObject
{
    public GameObject LevelPrefab;

    public string DisplayName;

    [TextArea(3, 5)]
    public string Description;
}
