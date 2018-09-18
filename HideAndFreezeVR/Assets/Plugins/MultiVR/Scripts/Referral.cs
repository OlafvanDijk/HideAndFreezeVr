using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A referral refers to a different object in the scene, which is probably in a different place.
/// When selected, you will be "referred" to the actual object.
/// </summary>
[ExecuteInEditMode]
public class Referral : MonoBehaviour
{
    public GameObject refersTo;
}
