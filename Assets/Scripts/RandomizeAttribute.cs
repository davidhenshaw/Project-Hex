using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAttribute : PropertyAttribute
{
    public float minValue;
    public float maxValue;

    public RandomizeAttribute(float min, float max)
    {
        minValue = min;
        maxValue = max;
    }
}
