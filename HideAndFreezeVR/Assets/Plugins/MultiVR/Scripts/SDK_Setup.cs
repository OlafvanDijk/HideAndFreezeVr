using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralized SDK setup for all SDK's to get info from.
/// </summary>
public class SDK_Setup : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The transform the final play area will be attached to.")]
    private Transform _playAreaRoot;

    /// <summary>
    /// The transform the final play area will be attached to.
    /// </summary>
    public Transform playAreaRoot
    {
        get
        {
            return _playAreaRoot;
        }
    }

    [SerializeField]
    [Tooltip("The play area alias that will be swapped with the SDK specific play area.")]
    private Transform _playAreaAttacher;

    /// <summary>
    /// The play area alias that will be swapped with the SDK specific play area.
    /// </summary>
    public Transform playAreaAttacher
    {
        get
        {
            return _playAreaAttacher;
        }
    }
}
