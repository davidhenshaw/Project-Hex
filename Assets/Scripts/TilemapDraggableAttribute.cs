using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapDraggableAttribute : MonoBehaviour
{
    public readonly Type interfaceType;

    public TilemapDraggableAttribute(Type type)
    {
        interfaceType = type;
    }
}
