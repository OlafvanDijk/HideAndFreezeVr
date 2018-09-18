using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerSwitcher : WaitForPlayAreaAwake
{
    [SerializeField]
    private VRTK_SDKManager _manager;

    [SerializeField]
    private ControllerSetup _default;

    [SerializeField]
    [Tooltip("If no specific setup exists, the default will be used.")]
    private SpecificControllerSetup[] _specificSetups;

    private ControllerSetup _current;

    private void Awake()
    {
        _current = _default;

        // Disable all other setups firstly.
        foreach (SpecificControllerSetup setup in _specificSetups)
        {
            setup.leftController.SetActive(false);
            setup.rightController.SetActive(false);
        }

        // And the default as well.
        _default.leftController.SetActive(false);
        _default.rightController.SetActive(false);
    }

    public override void OnPlayAreaAwake(PlayArea playArea)
    {
        SpecificControllerSetup setup = _findSetup(playArea);

        if (setup != null)
            _switchTo(setup);
    }

    /// <summary>
    /// Switch to a new setup, should only be called once.
    /// </summary>
    /// <param name="setup"></param>
    private void _switchTo(SpecificControllerSetup setup)
    {
#if UNITY_EDITOR
        string setupNames = "";

        foreach (PlayAreaType type in setup.playAreaTypes)
        {
            if (setupNames.Length > 0)
                setupNames += ", ";

            setupNames += Enum.GetName(typeof(PlayAreaType), type);
        }

        Debug.Log("Switching to controller setup for {" + setupNames + "}");
#endif

        GameObject currentLeft = _current.leftController;
        GameObject currentRight = _current.rightController;

        GameObject newLeft = setup.leftController;
        GameObject newRight = setup.rightController;

        // Replace the transforms
        _cloneTransform(currentLeft.transform, newLeft.transform);
        _cloneTransform(currentRight.transform, newRight.transform);

        // Change script aliases.
        _manager.scriptAliasLeftController = newLeft;
        _manager.scriptAliasRightController = newRight;

        // Enable the setup
        newLeft.SetActive(true);
        newRight.SetActive(true);
    }

    private void _cloneTransform(Transform from, Transform to)
    {
        // Copy transform values.
        to.position = from.position;
        to.rotation = from.rotation;
        to.localScale = from.localScale;

        to.SetParent(from.parent, true);

        List<Transform> children = new List<Transform>();

        foreach (Transform child in from)
        {
            children.Add(child); // First add them to an intermediate to avoid concurrent modification.
        }

        foreach (Transform child in children)
        {
            child.SetParent(to, true); // Replace parents.
        }

        // Disable old controller.
        from.gameObject.SetActive(false);
    }

    private SpecificControllerSetup _findSetup(PlayArea playArea)
    {
        foreach (SpecificControllerSetup setup in _specificSetups)
        {
            bool hasType = false;

            foreach (PlayAreaType type in setup.playAreaTypes)
            {
                if (type == playArea.type)
                {
                    hasType = true;
                    break;
                }
            }

            if (!hasType)
                continue;

            return setup;
        }

        return null;
    }
}

[Serializable]
public class ControllerSetup
{
    public GameObject leftController;
    public GameObject rightController;
}

[Serializable]
public class SpecificControllerSetup : ControllerSetup
{
    public PlayAreaType[] playAreaTypes;
}
