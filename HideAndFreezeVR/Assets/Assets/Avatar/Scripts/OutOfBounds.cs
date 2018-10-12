using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour {

    [SerializeField]
    private string tag;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Use this for initialization
    void Start () {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

	}

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(tag))
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
            this.transform.position = originalPosition;
            this.transform.rotation = originalRotation;
        }
    }
}
