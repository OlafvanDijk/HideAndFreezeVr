using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Identifier script for the play area alias.
/// </summary>
public class PlayAreaAlias : MultiVRAlias
{
    private PlayArea _playArea;

    /// <summary>
    /// The actual play area.
    /// </summary>
    public PlayArea playArea
    {
        get
        {
            if (_playArea == null)
                _playArea = GetComponentInChildren<PlayArea>();

            Debug.Log(_playArea);
            return _playArea;
        }
    }
}
