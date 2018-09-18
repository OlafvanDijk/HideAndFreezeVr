using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Identifier script for the head alias.
/// </summary>
public class HeadAlias : MultiVRAlias
{
    [SerializeField]
    private Transform _avatarOffset;

    /// <summary>
    /// Offset transform from the head where the avatar's head should be.
    /// </summary>
    public Transform avatarOffset
    {
        get
        {
            return _avatarOffset;
        }
    }
}
