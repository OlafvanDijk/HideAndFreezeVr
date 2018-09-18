using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayAreaBoundaryReference : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("MultiVR/Reference Boundary/SteamVR")]
    private static void _setReferenceBoundarySteam()
    {
        MultiVRSetup.instance.referenceBoundary = PlayAreaType.STEAM;
    }

    [MenuItem("MultiVR/Reference Boundary/Oculus")]
    private static void _setReferenceBoundaryOculus()
    {
        MultiVRSetup.instance.referenceBoundary = PlayAreaType.OCULUS;
    }

    [MenuItem("MultiVR/Reference Boundary/GearVR")]
    private static void _setReferenceBoundaryGear()
    {
        MultiVRSetup.instance.referenceBoundary = PlayAreaType.GEAR;
    }

    [MenuItem("MultiVR/Reference Boundary/Simulated")]
    private static void _setReferenceBoundarySimulated()
    {
        MultiVRSetup.instance.referenceBoundary = PlayAreaType.SIMULATED;
    }
#endif

    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private MeshFilter _filter;

    [SerializeField]
    private BoxCollider _box;

    [SerializeField]
    private PlayAreaType[] _typesReferenced;

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        PlayAreaType enabledType = MultiVRSetup.instance.referenceBoundary;
        bool isEnabled = false;

        foreach (PlayAreaType type in _typesReferenced)
        {
            if (type == enabledType)
            {
                isEnabled = true;
                break;
            }
        }

        if (!isEnabled) // Only draw this gizmo if the reference is set to this play area type.
            return;

        if (_box != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawWireCube(_box.center, _box.size);
        }
    }
#endif
}

