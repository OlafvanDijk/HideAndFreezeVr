using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public enum HandSide
{
    Left,
    Right
}

/// <summary>
/// Identifier script for the hand alias.
/// </summary>
public class HandAlias : MultiVRAlias
{
    private GameObject _controller;

    /// <summary>
    /// The VRTK Controller, a sibiling on this alias.
    /// Could be null before the play area is loaded.
    /// </summary>
    public GameObject controller
    {
        get
        {
            if (_controller == null)
            {
                MultiVRSetup setup = GetComponentInParent<MultiVRSetup>();

                if (setup != null)
                {
                    VRTK_SDKManager manager = setup.GetComponentInChildren<VRTK_SDKManager>();

                    if (manager != null)
                    {
                        _controller = _side == HandSide.Left ? manager.scriptAliasLeftController : manager.scriptAliasRightController;
                    }
                }
            }

            return _controller;
        }
    }

    [SerializeField]
    private HandSide _side;

    /// <summary>
    /// What hand are we?
    /// </summary>
    public HandSide side
    {
        get
        {
            return _side;
        }
    }

    [SerializeField]
    private Transform _avatarOffset;

    /// <summary>
    /// Offset transform from the hand where the avatar's hand should be.
    /// </summary>
    public Transform avatarOffset
    {
        get
        {
            return _avatarOffset;
        }
    }
}
