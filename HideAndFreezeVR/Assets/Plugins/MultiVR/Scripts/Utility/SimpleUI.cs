using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Simple User Interface Script, allows you to easily make buttons and a custom editor without having to write a custom editor script.
/// </summary>
public class SimpleUI : MonoBehaviour
{
    /* Example:

    [SerializeField]
    [Order(0)]
    private int _field = 0;

    [Button("Test Button")]
    [Order(1)]
    private void _testButton()
    {
    
    }

    */

    /// <summary>
    /// Attribute that when attached to a method with no parameters, shows it as a button in the editor.
    /// </summary>
    public class ButtonAttribute : Attribute
    {
        public ButtonAttribute() : this(null)
        {

        }

        /// <summary>
        /// Display the button with a specific title.
        /// </summary>
        /// <param name="buttonTitle"></param>
        public ButtonAttribute(string buttonTitle)
        {
            this.buttonTitle = buttonTitle;
        }

        public readonly string buttonTitle;
    }

    /// <summary>
    /// Unity knows the order of fields by reading them during compilation.
    /// Sadly we do not have this technology, so if you want to manually order anything, use this attribute, lower indices start at the top of the editor.
    /// </summary>
    public class OrderAttribute : Attribute
    {
        public OrderAttribute(int index)
        {
            _declarationIndex = index;
        }

        private int _declarationIndex = 0;

        public int declarationIndex
        {
            get
            {
                return _declarationIndex;
            }
        }
    }
}