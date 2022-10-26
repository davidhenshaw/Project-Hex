using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIScreen : UIScreen
{
    public GameObject screenspaceMarkers;
    public GameObject markerWidgetPrefab;

    public UIMarkerWidget CreateUIMarker(Transform target, Sprite icon)
    {
        var widgetObj = Instantiate(markerWidgetPrefab, screenspaceMarkers.transform);

        var widget = widgetObj.GetComponent<UIMarkerWidget>();
        widget.SetIcon(icon);
        widget.worldspaceTransform = target;

        return widget;
    }
}
