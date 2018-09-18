/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleUI), true)]
[CanEditMultipleObjects]
public class SimpleUIEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        SimpleUI ui = target as SimpleUI;
        SerializedObject serializedObject = new SerializedObject(ui);

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        object[] emptyArgs = new object[0];

        List<MemberInfo> members = new List<MemberInfo>(target.GetType().GetMembers(flags));

        members.Sort((a, b) =>
        {
            SimpleUI.OrderAttribute orderAttrA = _getAttribute<SimpleUI.OrderAttribute>(a.GetCustomAttributes(typeof(SimpleUI.OrderAttribute), true));
            SimpleUI.OrderAttribute orderAttrB = _getAttribute<SimpleUI.OrderAttribute>(b.GetCustomAttributes(typeof(SimpleUI.OrderAttribute), true));

            bool aNull = orderAttrA == null;
            bool bNull = orderAttrB == null;

            if (aNull || bNull)
            {
                if (aNull && bNull)
                    return 0;
                else if (aNull)
                    return -1;
                else // bNull
                    return 1;
            }

            return orderAttrA.declarationIndex - orderAttrB.declarationIndex;
        });

        foreach (MemberInfo info in members)
        {
            object[] attributes = info.GetCustomAttributes(true);

            Func<Type, object> getAttribute = (type) =>
            {
                return _getAttribute(type, attributes);
            };

            SimpleUI.OrderAttribute order = (SimpleUI.OrderAttribute)getAttribute(typeof(SimpleUI.OrderAttribute));

            // Method specific attributes
            if (info is MethodInfo)
            {
                MethodInfo methodInfo = info as MethodInfo;

                SimpleUI.ButtonAttribute button = (SimpleUI.ButtonAttribute)getAttribute(typeof(SimpleUI.ButtonAttribute));

                if (button != null)
                {
                    if (GUILayout.Button(button.buttonTitle == null ? info.Name : button.buttonTitle))
                    {
                        methodInfo.Invoke(ui, emptyArgs);
                    }
                }
            }
            
            // Field specific attributes
            if (info is FieldInfo)
            {
                FieldInfo fieldInfo = info as FieldInfo;

                if (fieldInfo.IsPrivate && getAttribute(typeof(SerializeField)) != null || fieldInfo.IsPublic && getAttribute(typeof(NonSerializedAttribute)) == null)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldInfo.Name));
                }
            }
        }
    }

    private T _getAttribute<T>(object[] attributes)
    {
        return (T)_getAttribute(typeof(T), attributes);
    }

    private object _getAttribute(Type type, object[] attributes)
    {
        foreach (object attribute in attributes)
        {
            if (attribute.GetType().IsAssignableFrom(type))
                return attribute;
        }

        return null;
    }
}
*/