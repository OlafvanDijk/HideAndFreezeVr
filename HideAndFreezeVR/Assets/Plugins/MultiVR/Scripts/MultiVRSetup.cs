using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main class for the whole MultiVR Setup
/// There should only be a single instance of this in the scene.
/// </summary>
[ExecuteInEditMode]
public class MultiVRSetup : MonoBehaviour
{
#region SINGLETON
    private static MultiVRSetup _instance;

    public static MultiVRSetup instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MultiVRSetup>();

                if (_instance != null)
                    return _instance;
                else
                    Debug.LogError("No MultiVR Setup is present in the scene but it was still accessed!");
            }

            return _instance;
        }
    }
    #endregion
    [SerializeField]
    private PlayAreaType _referenceBoundary = PlayAreaType.STEAM;

    public PlayAreaType referenceBoundary
    {
        get
        {
            return _referenceBoundary;
        }

        set
        {
            _referenceBoundary = value;
        }
    }

    [SerializeField]
    private HeadAlias _headAlias;

    public HeadAlias headAlias
    {
        get
        {
            return _headAlias;
        }
    }

    [SerializeField]
    private PlayAreaAlias _playAreaAlias;

    public PlayAreaAlias playAreaAlias
    {
        get
        {
            return _playAreaAlias;
        }
    }

    [SerializeField]
    private HandAlias _leftHandAlias;

    public HandAlias leftHandAlias
    {
        get
        {
            return _leftHandAlias;
        }
    }

    [SerializeField]
    private HandAlias _rightHandAlias;

    public HandAlias rightHandAlias
    {
        get
        {
            return _rightHandAlias;
        }
    }

    private void OnEnable()
    {
        _instance = this; // Set singleton instance to this.

        if (!Application.isPlaying)
            return;

        headAlias.setup = this;
        playAreaAlias.setup = this;
        leftHandAlias.setup = this;
        rightHandAlias.setup = this;
    }
}

public class MultiVRAlias : MonoBehaviour
{
    private MultiVRSetup _setup;

    /// <summary>
    /// Get the MultiVRSetup that this alias is attached to.
    /// </summary>
    public MultiVRSetup setup
    {
        get
        {
            return _setup;
        }

        set
        {
            if (_setup == null)
                _setup = value;
            else
                Debug.LogError("Can not set a multivr setup."); // We have to do this since there's no other way for us to pass the variable.
        }
    }
}