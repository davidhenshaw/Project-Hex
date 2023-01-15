using metakazz.Hex;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSelectorWidget : MonoBehaviour
{
    public event Action<HexDirection> Highlighted;
    public event Action<HexDirection> Selected;

    Dictionary<WorldSpaceButton, HexDirection> _dirButtons = new Dictionary<WorldSpaceButton, HexDirection>();

    [SerializeField] WorldSpaceButton _northButton;
    [SerializeField] WorldSpaceButton _northEastButton;
    [SerializeField] WorldSpaceButton _southEastButton;
    [SerializeField] WorldSpaceButton _southButton;
    [SerializeField] WorldSpaceButton _southWestButton;
    [SerializeField] WorldSpaceButton _northWestButton;

    // Start is called before the first frame update
    void Start()
    {
        foreach(WorldSpaceButton button in GetComponentsInChildren<WorldSpaceButton>())
        {
            button.MouseDown += OnSelected;
            button.MouseEnter += OnHighlighted;
        }

        _dirButtons.Add(_northButton, HexDirection.NORTH);
        _dirButtons.Add(_northEastButton, HexDirection.NORTHEAST);
        _dirButtons.Add(_southEastButton, HexDirection.SOUTHEAST);
        _dirButtons.Add(_southButton, HexDirection.SOUTH);
        _dirButtons.Add(_southWestButton, HexDirection.SOUTHWEST);
        _dirButtons.Add(_northWestButton, HexDirection.NORTHWEST);
    }

    private void OnSelected(WorldSpaceButton button)
    {
        Selected?.Invoke(_dirButtons[button]);
    }

    private void OnHighlighted(WorldSpaceButton button)
    {
        Highlighted?.Invoke(_dirButtons[button]);

    }
}
