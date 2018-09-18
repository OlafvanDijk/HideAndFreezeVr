using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// COMPLEX NUMBERS:
/// The classes in here are meant to be used as flexible replacements for their normal counterparts.
/// ie. Vector3 can be seamlessly replaced by ComplexVector3
/// The classes give more functionality than their normal counterparts, namely randomization and uniformity.
/// You should imagine it as redefining a number with a single outcome,
/// to a number that can have different outputs based on needs of the designer. (This can all be changed in the editor)
/// They might want a constant now, but a random number later, or have the vector be uniform.
/// If you have to change up your scripts every time, or even worse, facilitate all of them, this will be a pain.
/// But with this, just pop in a complex number, and your worries are over.

/// <summary>
/// Complex Vector3 that can take on multiple resulting states.
/// 
/// It can be:
///  - A normal, non-uniform Vector3
///  - A uniform Vector3 (set by a single float)
///  - A randomized, non-uniform Vector3 (set by two Vector3's)
///  - A randomized, uniform Vector3 (set by two floats)
///  
/// Uniformity is set by the isUniform flag.
/// Randomization is set by the isRandom flag.
/// 
/// Comes with a CustomPropertyRenderer to make it easy to use in the editor.
/// </summary>
[System.Serializable]
public class ComplexVector3
{
    /// <summary>
    /// Creates a Vector3 for an uniform value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    internal static Vector3 _uniformValue(float value)
    {
        return new Vector3(value, value, value);
    }

    public static ComplexVector3 Uniform(float value)
    {
        ComplexVector3 vec = new ComplexVector3();

        vec._valueUniform = value;
        vec._valueUniformMax = value;

        vec._isUniform = true;

        return vec;
    }

    public static ComplexVector3 UniformRandom(float min, float max)
    {
        ComplexVector3 vec = new ComplexVector3();

        vec._valueUniform = min;
        vec._valueUniformMax = max;

        vec._isUniform = true;
        vec._isRandom = true;

        return vec;
    }

    public static ComplexVector3 NonUniform(Vector3 value)
    {
        ComplexVector3 vec = new ComplexVector3();

        vec._value = value;
        vec._valueMax = value;

        return vec;
    }

    public static ComplexVector3 NonUniformRandom(Vector3 min, Vector3 max)
    {
        ComplexVector3 vec = new ComplexVector3();

        vec._value = min;
        vec._valueMax = max;

        vec._isRandom = true;

        return vec;
    }

    // If the target is uniform, the value is set to all components for every Vector3.
    // Note that the names of the fields are CRITICAL, they are shared between all Complex classes so they can all use the same property drawer.

    [SerializeField]
    internal Vector3 _value; // Also the minimum value if randomized.

    [SerializeField]
    internal float _valueUniform; // Also the minimum value if randomized.

    [SerializeField]
    internal Vector3 _valueMax; // Maximum value, only used when randomized.

    [SerializeField]
    internal float _valueUniformMax; // Also the minimum value if randomized.

    [SerializeField]
    internal bool _isUniform = false;

    [SerializeField]
    internal bool _isRandom = false;

    /// <summary>
    /// Set this ComplexVector3 to a specific value.
    /// </summary>
    /// <param name="vec"></param>
    public void Set(ComplexVector3 vec)
    {
        // We should only set the relevant values so that the other values are cached whenever we change back to them in the editor.
        if (vec._isUniform)
        {
            _valueUniform = vec._valueUniform;

            if (vec._isRandom)
                _valueUniformMax = vec._valueUniformMax;
        }
        else
        {
            _value = vec._value;

            if (vec._isRandom)
                _valueMax = vec._valueMax;
        }

        _isUniform = vec._isUniform;
        _isRandom = vec._isRandom;
    }

    public static implicit operator ComplexVector3(Vector3 vec)
    {
        return ComplexVector3.NonUniform(vec);
    }

    public static implicit operator ComplexVector3(float val)
    {
        return ComplexVector3.Uniform(val);
    }

    public static implicit operator Vector3(ComplexVector3 prop)
    {
        return prop.value;
    }

    /// <summary>
    /// Get the resulting value of this ComplexVector3
    /// </summary>
    public Vector3 value
    {
        get
        {
            if (!_isRandom)
            {
                if (_isUniform)
                {
                    return _uniformValue(_valueUniform);
                }

                return _value;
            }
            else
            {
                if (_isUniform)
                {
                    float randomUniform = Random.Range(_valueUniform, _valueUniformMax); // All field should have the same value so we can just use x.

                    return _uniformValue(randomUniform);
                }
                else
                {
                    float randomX = Random.Range(_value.x, _valueMax.x);
                    float randomY = Random.Range(_value.y, _valueMax.y);
                    float randomZ = Random.Range(_value.z, _valueMax.z);

                    return new Vector3(randomX, randomY, randomZ);
                }
            }
        }
    }
}

/// <summary>
/// Complex Vector2 that can take on multiple resulting states.
/// 
/// It can be:
///  - A normal, non-uniform Vector2.
///  - A uniform Vector2. (set by a single float)
///  - A randomized, non-uniform Vector2. (set by two Vector2's)
///  - A randomized, uniform Vector2. (set by two floats)
///  
/// Uniformity is set by the isUniform flag.
/// Randomization is set by the isRandom flag.
/// 
/// Comes with a CustomPropertyRenderer to make it easy to use in the editor.
/// </summary>
[System.Serializable]
public class ComplexVector2
{
    /// <summary>
    /// Creates a Vector2 for an uniform value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    internal static Vector2 _uniformValue(float value)
    {
        return new Vector2(value, value);
    }

    public static ComplexVector2 Uniform(float value)
    {
        ComplexVector2 vec = new ComplexVector2();

        vec._valueUniform = value;
        vec._valueUniformMax = value;

        vec._isUniform = true;

        return vec;
    }

    public static ComplexVector2 UniformRandom(float min, float max)
    {
        ComplexVector2 vec = new ComplexVector2();

        vec._valueUniform = min;
        vec._valueUniformMax = max;

        vec._isUniform = true;
        vec._isRandom = true;

        return vec;
    }

    public static ComplexVector2 NonUniform(Vector2 value)
    {
        ComplexVector2 vec = new ComplexVector2();

        vec._value = value;
        vec._valueMax = value;

        return vec;
    }

    public static ComplexVector2 NonUniformRandom(Vector2 min, Vector2 max)
    {
        ComplexVector2 vec = new ComplexVector2();

        vec._value = min;
        vec._valueMax = max;

        vec._isRandom = true;

        return vec;
    }

    // If the target is uniform, the value is set to all components for every Vector2.
    // Note that the names of the fields are CRITICAL, they are shared between all Complex classes so they can all use the same property drawer.

    [SerializeField]
    internal Vector2 _value; // Also the minimum value if randomized.

    [SerializeField]
    internal float _valueUniform; // Also the minimum value if randomized.

    [SerializeField]
    internal Vector2 _valueMax; // Maximum value, only used when randomized.

    [SerializeField]
    internal float _valueUniformMax; // Also the minimum value if randomized.

    [SerializeField]
    internal bool _isUniform = false;

    [SerializeField]
    internal bool _isRandom = false;

    /// <summary>
    /// Set this ComplexVector2 to a specific value.
    /// </summary>
    /// <param name="vec"></param>
    public void Set(ComplexVector2 vec)
    {
        // We should only set the relevant values so that the other values are cached whenever we change back to them in the editor.
        if (vec._isUniform)
        {
            _valueUniform = vec._valueUniform;

            if (vec._isRandom)
                _valueUniformMax = vec._valueUniformMax;
        }
        else
        {
            _value = vec._value;

            if (vec._isRandom)
                _valueMax = vec._valueMax;
        }

        _isUniform = vec._isUniform;
        _isRandom = vec._isRandom;
    }

    public static implicit operator ComplexVector2(Vector4 vec)
    {
        return ComplexVector2.NonUniformRandom(new Vector2(vec.x, vec.y), new Vector2(vec.z, vec.w));
    }

    public static implicit operator ComplexVector2(Vector2 vec)
    {
        return ComplexVector2.NonUniform(vec);
    }

    public static implicit operator ComplexVector2(float val)
    {
        return ComplexVector2.Uniform(val);
    }

    public static implicit operator Vector2(ComplexVector2 prop)
    {
        return prop.value;
    }

    /// <summary>
    /// Get the resulting value of this ComplexVector2.
    /// </summary>
    public Vector2 value
    {
        get
        {
            if (!_isRandom)
            {
                if (_isUniform)
                {
                    return _uniformValue(_valueUniform);
                }

                return _value;
            }
            else
            {
                if (_isUniform)
                {
                    float randomUniform = Random.Range(_valueUniform, _valueUniformMax); // All field should have the same value so we can just use x.

                    return _uniformValue(randomUniform);
                }
                else
                {
                    float randomX = Random.Range(_value.x, _valueMax.x);
                    float randomY = Random.Range(_value.y, _valueMax.y);

                    return new Vector2(randomX, randomY);
                }
            }
        }
    }
}

/// <summary>
/// Complex Float that can take on multiple resulting states.
/// 
/// It can be:
///  - A normal float.
///  - A randomized float.
///  
/// Randomization is set by the isRandom flag.
/// 
/// Comes with a CustomPropertyRenderer to make it easy to use in the editor.
/// </summary>
[System.Serializable]
public class ComplexFloat
{
    public static ComplexFloat Float(float value)
    {
        ComplexFloat num = new ComplexFloat();

        num._value = value;

        return num;
    }

    public static ComplexFloat Random(float min, float max)
    {
        ComplexFloat num = new ComplexFloat();

        num._value = min;
        num._valueMax = max;

        num._isRandom = true;

        return num;
    }
    
    // Note that the names of the fields are CRITICAL, they are shared between all Complex classes so they can all use the same property drawer.

    [SerializeField]
    internal float _value; // Also the minimum value if randomized.

    [SerializeField]
    internal float _valueMax; // Maximum value, only used when randomized.

    [SerializeField]
    internal bool _isUniform = false; // Should always stay false for ComplexFloat's so that the property drawer uses value and valueMax, not valueUniform and valueUniformMax (which don't exist here)

    [SerializeField]
    internal bool _isRandom = false;

    /// <summary>
    /// Set this ComplexFloat to a specific value.
    /// </summary>
    /// <param name="vec"></param>
    public void Set(ComplexFloat value)
    {
        // We should only set the relevant values so that the other values are cached whenever we change back to them in the editor.
        _value = value._value;

        if (value._isRandom)
        {
            _valueMax = value._valueMax;
        }

        _isRandom = value._isRandom;
    }

    public static implicit operator ComplexFloat(int value)
    {
        return ComplexFloat.Float(value);
    }

    public static implicit operator ComplexFloat(float value)
    {
        return ComplexFloat.Float(value);
    }

    public static implicit operator ComplexFloat(Vector2 value)
    {
        return ComplexFloat.Random(value.x, value.y);
    }

    public static implicit operator float(ComplexFloat prop)
    {
        return prop.value;
    }

    /// <summary>
    /// Get the resulting value of this ComplexFloat
    /// </summary>
    public float value
    {
        get
        {
            if (!_isRandom)
            {
                return _value;
            }
            else
            {
                return UnityEngine.Random.Range(_value, _valueMax);
            }
        }
    }
}

/// <summary>
/// Complex Int that can take on multiple resulting states.
/// 
/// It can be:
///  - A normal int.
///  - A randomized int.
/// 
///  
/// Randomization is set by the isRandom flag.
/// 
/// Comes with a CustomPropertyRenderer to make it easy to use in the editor.
/// </summary>
[System.Serializable]
public class ComplexInt
{
    public static ComplexInt Int(int value)
    {
        ComplexInt num = new ComplexInt();

        num._value = value;

        return num;
    }

    public static ComplexInt Random(int min, int max)
    {
        ComplexInt num = new ComplexInt();

        num._value = min;
        num._valueMax = max;

        num._isRandom = true;

        return num;
    }

    // Note that the names of the fields are CRITICAL, they are shared between all Complex classes so they can all use the same property drawer.

    [SerializeField]
    internal int _value; // Also the minimum value if randomized.

    [SerializeField]
    internal int _valueMax; // Maximum value, only used when randomized.

    [SerializeField]
    internal bool _isUniform = false; // Should always stay false for ComplexFloat's so that the property drawer uses value and valueMax, not valueUniform and valueUniformMax (which don't exist here)

    [SerializeField]
    internal bool _isRandom = false;

    /// <summary>
    /// Set this ComplexInt to a specific value.
    /// </summary>
    /// <param name="vec"></param>
    public void Set(ComplexInt value)
    {
        // We should only set the relevant values so that the other values are cached whenever we change back to them in the editor.
        _value = value._value;

        if (value._isRandom)
        {
            _valueMax = value._valueMax;
        }

        _isRandom = value._isRandom;
    }

    public static implicit operator ComplexInt(int value)
    {
        return ComplexInt.Int(value);
    }

    public static explicit operator ComplexInt(float value)
    {
        return ComplexInt.Int((int)value);
    }

    public static implicit operator ComplexInt(Vector2 value)
    {
        return ComplexInt.Random((int)value.x, (int)value.y);
    }

    public static implicit operator int(ComplexInt prop)
    {
        return prop.value;
    }

    /// <summary>
    /// Get the resulting value of this ComplexInt
    /// </summary>
    public int value
    {
        get
        {
            if (!_isRandom)
            {
                return _value;
            }
            else
            {
                return UnityEngine.Random.Range(_value, _valueMax + 1); // We want the max value to be inclusive.
            }
        }
    }
}

#if UNITY_EDITOR

/// <summary>
/// Property drawer for all complex numbers.
/// This class is kind of messy because of unity's questionable, required use of hardcoded numbers.
/// </summary>
[CustomPropertyDrawer(typeof(ComplexVector3))]
[CustomPropertyDrawer(typeof(ComplexVector2))]
[CustomPropertyDrawer(typeof(ComplexFloat))]
[CustomPropertyDrawer(typeof(ComplexInt))]
public class ComplexVector3Drawer : PropertyDrawer
{
    public float labelWidth
    {
        get
        {
            return EditorGUIUtility.labelWidth;
        }

        set
        {
            EditorGUIUtility.labelWidth = value;
        }
    }

    /// Different sizes:
    /// The smallest size is one or two lines, depending on if we have a random value.
    /// The medium size is two or three lines, it's when there is too little space to have the random/uniform toggles on the same lines as the first value.
    /// The full size is two to five lines, it's when Vector2/3 fields expand to take two lines. 

    public bool fullSize
    {
        get
        {
            return Screen.width < 335;
        }
    }

    public bool smallestSize
    {
        get
        {
            return Screen.width >= 538;
        }
    }

    public bool mediumSize
    {
        get
        {
            return !smallestSize;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty isRandom = property.FindPropertyRelative("_isRandom");
        SerializedProperty isUniform = property.FindPropertyRelative("_isUniform");

        bool isRandomValue = isRandom.boolValue;
        bool isUniformValue = isUniform.boolValue;

        int lines = (isRandomValue ? 2 : 1);

        if (fullSize) // Uses full lines for the Vector3 drawers, so we need a lot more lines.
            lines += isRandomValue ? (isUniformValue ? 2 : 4): 2;
        else if (mediumSize)
            lines += 1;

        return lines * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Cached values
        bool fullSize = this.fullSize;
        bool smallestSize = this.smallestSize;
        bool mediumSize = this.mediumSize;

        // Constants
        float singleLineHeight = EditorGUIUtility.singleLineHeight;
        float extraWidth = (Screen.width - 277); // Minimum width of the GUI is 277 pixels. (Why that number? no clue)

        // Properties
        SerializedProperty isRandom = property.FindPropertyRelative("_isRandom");
        SerializedProperty isUniform = property.FindPropertyRelative("_isUniform");

        SerializedProperty min = property.FindPropertyRelative("_value");
        SerializedProperty max = property.FindPropertyRelative("_valueMax");

        SerializedProperty minUniform = property.FindPropertyRelative("_valueUniform");
        SerializedProperty maxUniform = property.FindPropertyRelative("_valueUniformMax");

        // Type tests, for identifying what complex number we're dealing with.
        bool isVector3 = min.propertyType == SerializedPropertyType.Vector3; // ComplexVector3
        bool isVector2 = min.propertyType == SerializedPropertyType.Vector2; // ComplexVector2

        bool isAnyVector = (isVector3 || isVector2);

        bool isFloat = min.propertyType == SerializedPropertyType.Float; // ComplexFloat
        bool isInt = min.propertyType == SerializedPropertyType.Integer; // ComplexInt

        string vectorComponents = isVector3 ? "XYZ" : "XY";

        // Cached properties
        bool isRandomValue = isRandom.boolValue;
        bool isUniformValue = isAnyVector ? isUniform.boolValue : false;

        // Identifying on the label what type you're dealing with.
        if (isRandomValue && isUniformValue)
            label.text += " (Uniform, Randomized)";
        else if (isRandomValue)
            label.text += " (Randomized)";
        else if (isUniformValue)
            label.text += " (Uniform)";

        string existingTooltip = "";

        TooltipAttribute[] tooltips = (TooltipAttribute[])fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), true);

        if (tooltips.Length > 0)
        {
            foreach (TooltipAttribute tooltip in tooltips)
            {
                // Append the tooltip, with a new line in front, since the first newline will be used to offset from the label text
                existingTooltip += System.Environment.NewLine + tooltip.tooltip;
            }
        }

        // Since the labels can get kind of long, set the tooltip to it so you can hover and see the full text.
        label.tooltip = label.text + existingTooltip;

        // If we're using full line mode, the prefix label can use... the full line.
        if (fullSize)
        {
            labelWidth = Screen.width;
        }

        // Place the prefix label
        Rect prefix = EditorGUI.PrefixLabel(position, label);

        // Offset between elements
        Vector2 offset = new Vector2(20, 0);

        labelWidth = 40;

        Rect rect = new Rect(prefix.position, Vector3.zero);

        float fullLinesIdent = 20;

        // If we use full lines, we need to move to the next line.
        if (fullSize)
        {
            rect.x = position.x + fullLinesIdent;
            rect.y += singleLineHeight;
        }

        Rect properties = rect;

        // If we use three lines, 
        if (mediumSize)
            rect.y += singleLineHeight;

        rect.size = new Vector2(Mathf.Min(Screen.width - rect.x, 200), singleLineHeight);

        if (fullSize)
        {
            rect.width = Screen.width - 20 - fullLinesIdent;
        }

        if (isUniformValue)
        {
            labelWidth = isRandomValue ? 70 : labelWidth;
            minUniform.floatValue = EditorGUI.FloatField(rect, new GUIContent(isRandomValue ? "Min " + vectorComponents + ":" : vectorComponents + ":"), minUniform.floatValue);
        }
        else
        {
            if (isVector3)
                min.vector3Value = EditorGUI.Vector3Field(rect, new GUIContent(isRandomValue ? "Min:" : ""), min.vector3Value);
            else if (isVector2)
                min.vector2Value = EditorGUI.Vector2Field(rect, new GUIContent(isRandomValue ? "Min:" : ""), min.vector2Value);
            else if (isFloat)
                min.floatValue = EditorGUI.FloatField(rect, new GUIContent(isRandomValue ? "Min:" : ""), min.floatValue);
            else if (isInt)
                min.intValue = EditorGUI.IntField(rect, new GUIContent(isRandomValue ? "Min:" : ""), min.intValue);
        }

        if (isRandomValue)
        {
            Rect nextLine = rect;
            nextLine.y += EditorGUIUtility.singleLineHeight;
            nextLine.size = new Vector2(Mathf.Min(Screen.width - nextLine.x, 200), singleLineHeight);

            if (fullSize)
            {
                nextLine.width = Screen.width - 20 - fullLinesIdent;

                if (!isUniformValue)
                    nextLine.y += EditorGUIUtility.singleLineHeight;
            }

            if (isUniformValue)
            {
                maxUniform.floatValue = EditorGUI.FloatField(nextLine, new GUIContent(isRandomValue ? "Max " + vectorComponents + ":" : vectorComponents + ":"), maxUniform.floatValue);
            }
            else
            {
                if (isVector3)
                    max.vector3Value = EditorGUI.Vector3Field(nextLine, new GUIContent(isRandomValue ? "Max:" : ""), max.vector3Value);
                else if (isVector2)
                    max.vector2Value = EditorGUI.Vector2Field(nextLine, new GUIContent(isRandomValue ? "Max:" : ""), max.vector2Value);
                else if (isFloat)
                    max.floatValue = EditorGUI.FloatField(nextLine, new GUIContent(isRandomValue ? "Max:" : ""), max.floatValue);
                else if (isInt)
                    max.intValue = EditorGUI.IntField(nextLine, new GUIContent(isRandomValue ? "Max:" : ""), max.intValue);
            }
        }

        bool writeFullNames = fullSize || mediumSize;

        labelWidth = writeFullNames ? 80 : 30;

        if (smallestSize)
            properties.x += rect.size.x + offset.x;

        properties.size = new Vector2(labelWidth + 20, singleLineHeight);
        isRandom.boolValue = EditorGUI.Toggle(properties, new GUIContent(writeFullNames ? "Randomized" : "RND", "Is the value randomized every time it is used?"), isRandom.boolValue);

        if (isAnyVector) // Only vectors have uniform values
        {
            properties.x += labelWidth + offset.x;
            labelWidth = writeFullNames ? 60 : 30;
            isUniform.boolValue = EditorGUI.Toggle(properties, new GUIContent(writeFullNames ? "Uniform" : "UNI", "Is the vector uniform, so should we use one value for all components? (ie. 1.4 = " + (isVector2 ? "Vector2(1.4, 1.4)" : "Vector3(1.4, 1.4, 1.4)") + ")"), isUniform.boolValue);
        }
    }
}

#endif
