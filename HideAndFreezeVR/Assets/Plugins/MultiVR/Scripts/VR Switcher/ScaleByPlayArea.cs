using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scale an object by the play area size.
/// To use it, save the bounds of the play area you tested it with.
/// As well as the scale that worked for that play area.
/// Now any play area should scale the object up or down depending on how large it is.
/// NOTE: This never does anything with y/height, since the room size is only defined by x/width and z/length
/// </summary>
public class ScaleByPlayArea : WaitForPlayAreaBoundary
{
    [Header("Read script description for how to")]
    [SerializeField]
    private Vector3 _recordedBoundsA;

    [SerializeField]
    private Vector3 _recordedScaleA;

    [SerializeField]
    private Vector3 _recordedBoundsB;

    [SerializeField]
    private Vector3 _recordedScaleB;

    [SerializeField]
    [ReadOnly]
    private float _scalePerWidth;

    [SerializeField]
    [ReadOnly]
    private float _scalePerLength;

    private void Start() // We need this to exist so we can disable this in the editor.
    {
        
    }

    public override void OnPlayAreaBoundaryInitialize(PlayAreaBoundary boundary)
    {
        if (!enabled)
            return;

        Vector3 oldSizeA = _recordedBoundsA;
        Vector3 oldSizeB = _recordedBoundsB;

        Vector3 diffSize = new Vector3(oldSizeB.x - oldSizeA.x, 0, oldSizeB.z - oldSizeA.z);

        Vector3 oldScaleA = _recordedScaleA;
        Vector3 oldScaleB = _recordedScaleB;

        Vector3 diffScale = new Vector3(oldScaleB.x - oldScaleA.x, 0, oldScaleB.z - oldScaleA.z);

        _scalePerWidth = (diffScale.x / diffSize.x);
        _scalePerLength = (diffScale.z / diffSize.z);

        Vector3 newSize = boundary.bounds.size;

        // The idea here is that the scale has an inverse relationship with the old size.
        // But because of that, we need a different starting point other than zero.
        // Otherwise we would always have a negative scale as a result.
        // The _scalePerWidth/Length are defined from oldScaleA's relationship with oldScaleB
        // Then we can relativate the new size to the oldSizeA, and scale it accordingly.
        float newX = oldScaleA.x + (_scalePerWidth * (newSize.x - oldSizeA.x));
        float newZ = oldScaleA.z + (_scalePerLength * (newSize.z - oldSizeA.z));

        transform.localScale = new Vector3(newX, transform.localScale.y, newZ);

        //transform.localScale = new Vector3(Mathf.Max(oldScaleA.x + (_scalePerWidth * newSize.x), 0), transform.localScale.y, Mathf.Max(oldScaleA.z + (_scalePerLength * newSize.z), 0));
    }
}
