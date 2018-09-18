using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

/// <summary>
/// Basically a multiline version of [Header], which doesn't bold.
/// </summary>
public class CommentAttribute : PropertyAttribute
{
    public CommentAttribute(string comment)
    {
        this.comment = comment;
    }

    public string comment;
}

/// <summary>
/// A property used on integers to create a layer selector.
/// This works the same as it being of type LayerMask, but with only a single layer allowed to be selected.
/// </summary>
public class LayerSelectorAttribute : PropertyAttribute
{

}

/// <summary>
/// Defines a variable that is not meant to be changed, but still gives useful information.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute
{

}

/// <summary>
/// Defines a variable that is clearly optional. This will add an (Optional) after the name.
/// </summary>
public class OptionalAttribute : PropertyAttribute
{

}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyRenderer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // Start drawing blocked out GUI
        EditorGUI.PropertyField(rect, property, label, true); // Draw like normal
        GUI.enabled = true; // Reset
    }
}

[CustomPropertyDrawer(typeof(OptionalAttribute))]
public class OptionalRenderer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        label.text += " (Optional)";

        EditorGUI.PropertyField(rect, property, label, true); // Draw like normal
    }
}

[CustomPropertyDrawer(typeof(LayerSelectorAttribute))]
public class LayerSelectorRenderer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(rect, GUIContent.none, property);

        if (property.propertyType != SerializedPropertyType.Integer)
        {
            label.text += " (Unsupported variable type for [LayerSelector], expected '" + SerializedPropertyType.Integer + "' but got '" + property.propertyType + "')";

            EditorGUI.LabelField(rect, label);
            return;
        }

        rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);

        property.intValue = EditorGUI.LayerField(rect, property.intValue);

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(CommentAttribute))]
public class CommentAttributeRenderer : PropertyDrawer
{
    private float SPACE_BEFORE
    {
        get
        {
            return EditorGUIUtility.singleLineHeight * 0.5f;
        }
    }

    private float SPACE_LINE
    {
        get
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

    private float SPACE_AFTER
    {
        get
        {
            return EditorGUIUtility.singleLineHeight * 1.5f;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        string[] lines = (attribute as CommentAttribute).comment.Split('\n');

        float length = 0f;

        length += SPACE_BEFORE;

        length += ((lines.Length - 1) * SPACE_LINE);

        length += SPACE_AFTER;

        return EditorGUI.GetPropertyHeight(property, label) + length;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var headerAttribute = attribute as CommentAttribute;
        var propertyHeight = position.height;

        position.y += SPACE_BEFORE;
        position.height = SPACE_LINE;

        string[] lines = headerAttribute.comment.Split('\n');

        foreach (string line in lines)
        {
            EditorGUI.LabelField(position, line, EditorStyles.label);

            position.y += SPACE_LINE;
        }

        position.y -= SPACE_LINE; // The last line was added without there being a next line to make it for.
        position.y += SPACE_AFTER;

        EditorGUI.PropertyField(position, property);

        position.height += propertyHeight;
    }
}

#endif