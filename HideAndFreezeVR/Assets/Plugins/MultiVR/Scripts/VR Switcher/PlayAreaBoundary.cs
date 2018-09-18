using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayAreaBoundary : WaitForPlayAreaAwake
{
    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private MeshFilter _filter;

    [SerializeField]
    private BoxCollider _box;

    [SerializeField]
    private bool _render = false;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    [Tooltip("Makes the play area boundary slightly smaller than it is in reality.")]
    private float _shrinkMultiplier = 0.9f;

    [SerializeField]
    [ReadOnly]
    private Bounds _bounds;

    /// <summary>
    /// The bounds that define the play area.
    /// </summary>
    public Bounds bounds
    {
        get
        {
            return _bounds;
        }
    }

    [SerializeField]
    private float _height = 2;

    [Header("Testing (Ignores Y)")]
    [SerializeField]
    private Vector3 _fakeSize;

    [SerializeField]
    private bool _useFakeSize = false;

    [SerializeField]
    private CenteredMode _fakeCentered = CenteredMode.DEFAULT;

    [SerializeField]
    [HideInInspector]
    private Mesh _mesh;

    public enum CenteredMode
    {
        FORCE_ON, DEFAULT, FORCE_OFF
    }

    public void OnValidate()
    {
        _renderer.enabled = _render;
    }

    public override void OnPlayAreaAwake(PlayArea playArea)
    {
        if (playArea.type == PlayAreaType.GEAR)
        {
            _box.size = new Vector3(0, 0, 0);
            _renderer.enabled = false;
            return;
        }

        SDK_BaseBoundaries b = playArea.SDK.boundariesSDK;

        bool center = playArea.type == PlayAreaType.OCULUS; // Should we center the play area?

        if (_fakeCentered != CenteredMode.DEFAULT)
            center = _fakeCentered == CenteredMode.FORCE_ON ? true : false;

        _box.enabled = true;

#if UNITY_EDITOR
        _mesh = _createMesh(b, _box, center); // Also makes the bounding box.
        _filter.mesh = _mesh;
        _renderer.enabled = _render;
#else
        _createBoundingBox(b.GetPlayAreaVertices(), _box, center);
        _renderer.enabled = false;
#endif

        Debug.Log(_box.bounds);

        StartCoroutine(_waitForBounds());
    }

    private IEnumerator _waitForBounds()
    {
        yield return new WaitForEndOfFrame();

        //if (_useFakeSize)
        //{
        //    Vector3 newSize = new Vector3(_fakeSize.x, _box.size.y, _fakeSize.z);

        //    //_bounds = new Bounds(_box.center, newSize);
        //    //_box.size = newSize;
        //}
        //else
        {
            _bounds = _box.bounds;
        }
        
        _box.enabled = false;

        // Let our listeners know we're here.
        foreach (WaitForPlayAreaBoundary wait in FindObjectsOfType<WaitForPlayAreaBoundary>())
        {
            wait.OnPlayAreaBoundaryInitialize(this);
        }
    }

    /// <summary>
    /// Create a mesh from the boundaries.
    /// </summary>
    /// <param name="boundaries"></param>
    /// <returns></returns>
    private Mesh _createMesh(SDK_BaseBoundaries boundaries, BoxCollider box, bool center)
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        Vector3? _firstVert = null;
        Vector3? _lastVert = null;

        List<Vector3> boundingBox = _createBoundingBox(boundaries.GetPlayAreaVertices(), box, center);

        Vector3 height = new Vector3(0, _height, 0);

        Vector2 uv00 = new Vector2(0, 0);
        Vector2 uv01 = new Vector2(0, 1);
        Vector2 uv10 = new Vector2(1, 0);
        Vector2 uv11 = new Vector2(1, 1);

        Action<Vector3, Vector3> createWall = (lastVec, vec) =>
        {
            // Create 2 triangles out of two floor vertices
            Vector3 v00 = lastVec;
            Vector3 v10 = vec;

            Vector3 v01 = v00 + height;
            Vector3 v11 = v10 + height;

            // Triangle 1 - Left Bottom -> Left Top -> Right Top
            vertices.Add(v00);
            vertices.Add(v01);
            vertices.Add(v11);

            int start = indices.Count;

            // Indices are flipped so that they face inwards
            indices.Add(start + 2);
            indices.Add(start + 1);
            indices.Add(start + 0);

            // UVs
            uvs.Add(uv00);
            uvs.Add(uv01);
            uvs.Add(uv11);

            // Triangle 2 - Left Bottom -> Right Top -> Right Bottom
            vertices.Add(v00);
            vertices.Add(v11);
            vertices.Add(v10);

            // Indices are flipped so that they face inwards
            indices.Add(start + 5);
            indices.Add(start + 4);
            indices.Add(start + 3);

            // UVs
            uvs.Add(uv00);
            uvs.Add(uv11);
            uvs.Add(uv10);
        };

        foreach (Vector3 vert in boundingBox)
        {
            if (_lastVert != null)
            {
                createWall(_lastVert.Value, vert);
            }

            if (_firstVert == null)
                _firstVert = vert;

            _lastVert = vert;
        }

        if (_lastVert != null && _firstVert != null)
            createWall(_lastVert.Value, _firstVert.Value); // Create the last wall connection

        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

        mesh.UploadMeshData(false);

        return mesh;
    }

    /// <summary>
    /// Cuts the vertices into 4 vertices (the bounding box for the play area)
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns></returns>
    private List<Vector3> _createBoundingBox(Vector3[] vertices, BoxCollider box, bool doCenter)
    {
        List<Vector3> boundingBox = new List<Vector3>();

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        foreach (Vector3 vert in vertices)
        {
            minX = Mathf.Min(minX, vert.x);
            maxX = Mathf.Max(maxX, vert.x);

            minZ = Mathf.Min(minZ, vert.z);
            maxZ = Mathf.Max(maxZ, vert.z);
        }

        float width = (maxX - minX);
        float height = _height;
        float length = (maxZ - minZ);

        if (_useFakeSize)
        {
            width = _fakeSize.x;
            length = _fakeSize.z;
        }
        else
        {
            float xChangeHalf = (width - (width * _shrinkMultiplier)) * 0.5f;
            float zChangeHalf = (length - (length * _shrinkMultiplier)) * 0.5f;

            // Both positions get moved closer to the center by half the shrinked size.
            minX += xChangeHalf;
            maxX -= xChangeHalf;

            minZ += zChangeHalf;
            maxZ -= zChangeHalf;

            width *= _shrinkMultiplier;
            length *= _shrinkMultiplier;
        }

        float halfWidth = (width * 0.5f);
        float halfHeight = height * 0.5f;
        float halfLength = (length * 0.5f);

        Vector3 center;

        if (doCenter)
        {
            center = new Vector3(0, halfHeight, 0);

            minX = -halfWidth;
            maxX = halfWidth;

            minZ = -halfLength;
            maxZ = halfLength;
        }
        else
        {
            center = new Vector3(minX + halfWidth, halfHeight, minZ + halfLength);
        }

        box.center = center;
        box.size = new Vector3(width, _height, length);

        boundingBox.Add(new Vector3(minX, 0, minZ));
        boundingBox.Add(new Vector3(maxX, 0, minZ));
        boundingBox.Add(new Vector3(maxX, 0, maxZ));
        boundingBox.Add(new Vector3(minX, 0, maxZ));

        return boundingBox;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (_box != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawWireCube(_box.center, _box.size);
        }
    }
#endif
}

/// <summary>
/// Abstract class that waits for the play area boundary to be initialized.
/// </summary>
public abstract class WaitForPlayAreaBoundary : MonoBehaviour
{
    /// <summary>
    /// Called when the play area boundary is initialized.
    /// </summary>
    /// <param name="boundary">The boundary for the sake of ease</param>
    public abstract void OnPlayAreaBoundaryInitialize(PlayAreaBoundary boundary);
}

