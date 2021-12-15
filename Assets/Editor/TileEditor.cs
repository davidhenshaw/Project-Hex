using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tile)), CanEditMultipleObjects]
public class TileEditor : Editor
{
    private void OnSceneGUI()
    {
        var tile = target as Tile;
        var transform = tile.transform;

        EditorGUI.BeginChangeCheck();
        Handles.color = Color.red;

        Handles.Label(transform.position,
            $"{tile.GetCoordinates().ToString()}");
        var newPos = Handles.PositionHandle(transform.position, Quaternion.identity);

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Move Tile");
            tile.SnapToNearestCell(newPos);
        }
    }
}
