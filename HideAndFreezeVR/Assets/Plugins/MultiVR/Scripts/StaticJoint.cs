using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A static joint makes sure that whenever this or the other object moves this joint will make the same movement.
/// This means that at any moment, the distance and offset between the connected objects and this one are static (hence the name).
/// </summary>
public class StaticJoint : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    [Tooltip("Should this joint wait only for this transform to change and not the target?")]
    private bool _oneWay = false;

    public bool oneWay
    {
        get
        {
            return _oneWay;
        }

        set
        {
            _oneWay = value;
        }
    }

    [SerializeField]
    private bool _rotations = false;

    private Vector3 _offset;

    private Vector3 _lastPosA; // A = this transform
    private Vector3 _lastPosB; // B = target transform

    private Quaternion _rotationOffsetA;
    private Quaternion _rotationOffsetB;

    private Quaternion _lastRotA;
    private Quaternion _lastRotB;

    public override string ToString()
    {
        string relationship = _oneWay ? "->" : "<->";

        return string.Format("Static Joint [{0} {1} {2}]", gameObject.name, relationship, _target != null ? _target.gameObject.name : "null");
    }

    private bool _approximately(Vector3 before, Vector3 after)
    {
        return Mathf.Approximately(before.x, after.x) && Mathf.Approximately(before.y, after.y) && Mathf.Approximately(before.z, after.z);
    }

    private bool _aMoved
    {
        get
        {
            Vector3 before = _lastPosA;
            Vector3 after = transform.position;

            return !_approximately(before, after);
        }
    }

    private bool _aRotated
    {
        get
        {
            Quaternion before = _lastRotA;
            Quaternion after = transform.rotation;

            return before != after;
        }
    }

    private bool _bMoved
    {
        get
        {
            Vector3 before = _lastPosB;
            Vector3 after = _target.position;

            return !_approximately(before, after);
        }
    }

    private bool _bRotated
    {
        get
        {
            Quaternion before = _lastRotB;
            Quaternion after = _target.rotation;

            return before != after;
        }
    }

    private void _savePositions()
    {
        _lastPosA = transform.position;
        _lastPosB = _target.position;

        _lastRotA = transform.rotation;
        _lastRotB = _target.rotation;
    }

    //private void OnEnable()
    //{
    //    Debug.Log(ToString() + " was ENABLED.");
    //}

    //private void OnDisable()
    //{
    //    Debug.Log(ToString() + " was DISABLED.");
    //}

    private void Start()
    {
        _offset = transform.InverseTransformDirection(_target.position - transform.position);

        _rotationOffsetA = Quaternion.Inverse(transform.rotation * Quaternion.Inverse(_target.rotation));
        _rotationOffsetB = Quaternion.Inverse(_target.rotation * Quaternion.Inverse(transform.rotation));

        _savePositions();
    }

    private void LateUpdate()
    {
        if (_aMoved)
        {
            //Debug.LogFormat("{0} moved from {2} to {3} (D:{4})! Updating {1}!", transform.gameObject.name, ToString(), _lastPosA, transform.position, Vector3.Distance(_lastPosA, transform.position));

            Vector3 newPosA = transform.position;
            Vector3 newPosB = newPosA + transform.TransformDirection(_offset);

            _target.position = newPosB;
            transform.position = newPosA;
        }
        else if (!_oneWay && _bMoved)
        {
            //Debug.LogFormat("{0} moved from {2} to {3} (D:{4})! Updating {1}!", transform.gameObject.name, ToString(), _lastPosB, _target.transform.position, Vector3.Distance(_lastPosB, _target.transform.position));

            Vector3 newPosB = _target.position;
            Vector3 newPosA = newPosB - transform.TransformDirection(_offset);

            transform.position = newPosA;
            _target.position = newPosB;
        }

        if (_rotations)
        {
            if (_aRotated)
            {
                Quaternion newRotA = transform.rotation;
                Quaternion newRotB = transform.rotation * _rotationOffsetA;

                _target.rotation = newRotB;
                transform.rotation = newRotA;
            }
            else if (!_oneWay && _bRotated)
            {
                Quaternion newRotA = _target.rotation * _rotationOffsetB;
                Quaternion newRotB = _target.rotation;

                transform.rotation = newRotA;
                _target.rotation = newRotB;
            }
        }

        _savePositions();
    }
}
