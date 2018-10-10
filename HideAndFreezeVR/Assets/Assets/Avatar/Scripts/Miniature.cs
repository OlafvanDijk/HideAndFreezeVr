using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Miniature : MonoBehaviour {

    private PhotonView photonView;
    public bool owner;
    private Collider voteCollider;
    private VRTK_InteractableObject interactable;
    private bool voted;

    private void Awake()
    {
        this.GetComponent<LocationData>().test(owner);
        this.photonView = GetComponent<PhotonView>();
        photonView.ownerId = PhotonNetwork.player.ID;
    }
    // Use this for initialization
    void Start () {
        if (photonView.isMine)
        {
            interactable = GetComponent<VRTK_InteractableObject>();
            interactable.isGrabbable = true;
            //this.isGrabbable = true;
        }
	}

    private void Update()
    {
        if (voteCollider != null && !interactable.IsGrabbed())
        {
            VoteLevelAndSeeker vote = voteCollider.GetComponent<VoteLevelAndSeeker>();
            if (vote != null && !voted)
            {
                vote.SendVote(photonView.owner.ID);
                voted = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        voteCollider = other;
    }

    private void OnTriggerExit(Collider other)
    {
        voteCollider = null;
        voted = false;
    }
}
