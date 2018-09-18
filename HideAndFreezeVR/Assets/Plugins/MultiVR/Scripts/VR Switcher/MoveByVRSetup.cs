using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByVRSetup : WaitForPlayAreaAwake
{
    [SerializeField]
    private MoveCommand[] _differences;

    private bool _executed = false;

    public void OnEnable()
    {
        _checkActive();
    }

    public void Awake()
    {
        _checkActive();
    }

    private void _checkActive()
    {
        if (PlayArea.wasInitialized)
            OnPlayAreaAwake(PlayArea.instance);
    }

    public override void OnPlayAreaAwake(PlayArea playArea)
    {
        if (_executed)
            return;

        _executed = true;

        MoveCommand command = _getCommand(playArea);

        if (command == null)
            return;

        List<Rigidbody> awake = new List<Rigidbody>();

        foreach (Rigidbody rigidbody in transform.GetComponentsInChildren<Rigidbody>())
        {
            if (!rigidbody.IsSleeping())
            {
                rigidbody.Sleep();
                awake.Add(rigidbody);
            }
        }

        command.targetTransform.CopyTo(transform);

        foreach (Rigidbody rigidbody in awake)
        {
            rigidbody.WakeUp();
        }
    }

    private MoveCommand _getCommand(PlayArea playArea)
    {
        foreach (MoveCommand diff in _differences)
        {
            bool contains = false;

            foreach (PlayAreaType type in diff.playTypes)
            {
                if (type == playArea.type)
                {
                    contains = true;
                    break;
                }
            }

            if (contains)
            {
                return diff;
            }
        }

        return null;
    }
}

[System.Serializable]
public class MoveCommand
{
    public PlayAreaType[] playTypes;

    public Transform targetTransform;
}
