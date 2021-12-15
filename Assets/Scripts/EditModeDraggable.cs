using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EditModeDraggable 
{
    void OnSelect();
    void OnDragStart(Vector3 pos);
    void OnDragEnd(Vector3 pos);
}
