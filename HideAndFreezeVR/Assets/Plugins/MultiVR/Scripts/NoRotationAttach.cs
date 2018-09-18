using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attaches an object to another, but without copying rotation.
/// </summary>
public class NoRotationAttach : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private void Awake()
    {
        MultiVRUtil.MakeReferral(gameObject);

        transform.SetParent(null);
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
        transform.localScale = _target.lossyScale; // Since we are a root object, just copy the lossy scale.
    }
}
