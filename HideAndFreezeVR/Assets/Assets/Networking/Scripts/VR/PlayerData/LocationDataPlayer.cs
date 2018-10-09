using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the data of the player's hands and head, easily accesible for other scripts.
/// </summary>
public class LocationDataPlayer : MonoBehaviour {

    public Transform leftHand;
    public Transform rightHand;
    public Transform head;

    private void Awake()
    {
        VR_PlayerNetwork.Instance.player = this;
    }
}
