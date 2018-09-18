using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VRTK;

/// <summary>
/// Changes the layer on any generated controller collider to a target layer.
/// This can only be used in conjunction with a VRTK_InteractTouch
/// </summary>
public class ChangeControllerColliderLayer : MonoBehaviour
{
    [SerializeField]
    private VRTK_InteractTouch _touch;

    [SerializeField]
    [LayerSelector]
    private int _layer;

    private List<Collider> _colliders = new List<Collider>();

    /// <summary>
    /// The colliders in this controller.
    /// </summary>
    public List<Collider> colliders
    {
        get
        {
            return _colliders;
        }
    }

    private void FixedUpdate()
    {
        List<Collider> collidersFound = new List<Collider>();

        foreach (VRTK_PlayerObject obj in _touch.GetComponentsInChildren<VRTK_PlayerObject>())
        {
            if (obj.objectType != VRTK_PlayerObject.ObjectTypes.Collider)
                continue;

            collidersFound.AddRange(obj.GetComponentsInChildren<Collider>());
        }

        if (collidersFound.Count == 0) // Colliders haven't been filled yet.
            return;

        _colliders = collidersFound;

        foreach (Collider collider in collidersFound)
        {
            foreach (Transform child in collider.GetComponentsInChildren<Transform>()) // Includes the object itself and any children.
            {
                child.gameObject.layer = _layer; // Recursively apply the layer.
            }
        }

        enabled = false;
    }
}
