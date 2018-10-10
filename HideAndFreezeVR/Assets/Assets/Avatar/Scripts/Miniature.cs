using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Miniature : MonoBehaviour {

    private PhotonView photonView;
    public bool owner;

    private void Awake()
    {
        this.GetComponent<LocationData>().test(owner);
        this.photonView = GetComponent<PhotonView>();
    }
    // Use this for initialization
    void Start () {
        if (photonView.isMine)
        {
            GetComponent<VRTK_InteractableObject>().isGrabbable = true;
            //this.isGrabbable = true;
        }
	}
}
