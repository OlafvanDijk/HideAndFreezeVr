using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach a game object when the player area awakes.
/// </summary>
public class AttachOnPlayAreaAwake : WaitForPlayAreaAwake
{
    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private bool _leaveReferral = true;

    public void Start()
    {
        //Debug.Log("Awake " + gameObject + " - " + PlayArea.wasInitialized, gameObject);

        if (PlayArea.wasInitialized)
            OnPlayAreaAwake(PlayArea.instance);
    }

    public override void OnPlayAreaAwake(PlayArea playArea)
    {
        if (_leaveReferral)
        {
            MultiVRUtil.MakeReferral(_target);
        }

        //Debug.Log("Attaching Play Area Awake to " + gameObject, gameObject);

        _target.transform.SetParent(transform);

        _target.transform.position = transform.position + _offset;
        _target.transform.rotation = transform.rotation;
    }
}
