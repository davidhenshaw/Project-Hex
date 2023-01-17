using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceButton : MonoBehaviour
{
    public event Action<WorldSpaceButton> MouseEnter;
    public event Action<WorldSpaceButton> MouseDown;
    public event Action<WorldSpaceButton> MouseUp;
    public event Action<WorldSpaceButton> MouseOver;

    public Color NormalColor;
    public Color MouseOverColor;

    SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _sprite.color = NormalColor;
    }

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

    private void OnMouseOver()
    {
        MouseOver?.Invoke(this);

        _sprite.color = MouseOverColor;
    }

    private void OnMouseExit()
    {
        _sprite.color = NormalColor;
    }
}
