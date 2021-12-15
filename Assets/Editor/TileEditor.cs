using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tile)), CanEditMultipleObjects]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Rect newLine = EditorGUILayout.BeginHorizontal();

        string buttonLabel = (targets.Length > 1) ? "Snap All To Grid" : "Snap To Grid";

        if (GUILayout.Button(buttonLabel, GUILayout.MinHeight(50), GUILayout.ExpandHeight(true)))
        {
            SnapAllTileTargets();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void SnapAllTileTargets()
    {
        var tiles = targets;

        foreach (Object obj in tiles)
        {
            var t = obj as Tile;
            t.SnapToNearestCell();
        }
    }

    private void OnSceneGUI()
    {
        var tile = target as Tile;
        var transform = tile.transform;

        Handles.Label(transform.position,
            $"{tile.GetCoordinates().ToString()}");
    }

}
