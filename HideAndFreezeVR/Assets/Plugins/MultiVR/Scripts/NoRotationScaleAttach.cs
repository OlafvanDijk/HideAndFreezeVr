using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attaches an object to another, but without copying rotation or scale.
/// </summary>
public class NoRotationScaleAttach : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The target of this attach, if left empty the parent will be taken (if there is any).")]
    private Transform _target;

    [SerializeField]
    private bool _keepWorldPosition = true;

    private void Awake()
    {
        MultiVRUtil.MakeReferral(gameObject);

        if (_target == null)
            _target = transform.parent;

        transform.SetParent(null, _keepWorldPosition);
    }

    private void LateUpdate()
    {
        if (_target == null) // Was our attachment deleted?
        {
            Destroy(this); // Destroy ourselves.
            return;
        }

        Vector3 pos = _target.position;

        transform.position = pos;
    }
}

