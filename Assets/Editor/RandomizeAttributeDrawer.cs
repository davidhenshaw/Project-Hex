using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RandomizeAttribute))]
public class RandomizeAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(property.propertyType != SerializedPropertyType.Float)
        {
            EditorGUI.LabelField(position, $"Couldn't display \"{label.text}\" because it isn't a float");
            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        Rect labelPosition = new Rect(position.x,
            position.y,
            position.width,
            16f);
        Rect buttonPosition = new Rect(position.x, 
            position.y + labelPosition.height, 
            position.width, 
            16f);

        EditorGUI.LabelField(position, label, new GUIContent(property.floatValue.ToString()));


        if(GUI.Button(buttonPosition, "Randomize"))
        {
            RandomizeAttribute randomizeAttribute = (RandomizeAttribute)attribute;
            property.floatValue = Random.Range(
                randomizeAttribute.minValue,
                randomizeAttribute.maxValue);
        }

        EditorGUI.EndProperty();
    }
}
