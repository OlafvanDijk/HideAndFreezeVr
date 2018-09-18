using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach a game object when this object awakes.
/// </summary>
public class AttachOnAwake : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private bool _leaveReferral = true;

    private void Awake()
    {
        if (_leaveReferral)
        {
            MultiVRUtil.MakeReferral(_target);
        }

        _target.transform.SetParent(transform);

        _target.transform.position = transform.position + _offset;
        _target.transform.rotation = transform.rotation;
    }
}
