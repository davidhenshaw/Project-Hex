using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceButton : MonoBehaviour
{
    public event Action<WorldSpaceButton> MouseEnter;
    public event Action<WorldSpaceButton> MouseDown;
    public event Action<WorldSpaceButton> MouseUp;

    private void OnMouseEnter()
    {
        MouseEnter?.Invoke(this);
    }

    private void OnMouseDown()
    {
        MouseDown?.Invoke(this);
    }

    private void OnMouseUp()
    {
        MouseUp?.Invoke(this);
    }
}
