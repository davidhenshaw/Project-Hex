using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUIScreen : UIScreen
{

    public GameObject screenspaceMarkers;
    public GameObject markerWidgetPrefab;
    [Space]
    public GameObject levelMessageArea;
    public TMP_Text levelMessage;
    
    [TextArea(2, 3)]
    [Tooltip("Appears when the level becomes impossible to complete from an action")]
    public string impossibleMessage;

    private void OnEnable()
    {
        GameEvents.Instance.GoalImpossible.AddListener(
            () => { StartCoroutine(OnGoalImpossible()); }
        );
    }

    IEnumerator OnGoalImpossible()
    {
        yield return new WaitForSeconds(1.5f);
        levelMessageArea.SetActive(true);
        levelMessage.text = impossibleMessage;
    }

    public UIMarkerWidget CreateUIMarker(Transform target, Sprite icon)
    {
        var widgetObj = Instantiate(markerWidgetPrefab, screenspaceMarkers.transform);

        var widget = widgetObj.GetComponent<UIMarkerWidget>();
        widget.SetIcon(icon);
        widget.worldspaceTransform = target;

        return widget;
    }

}
