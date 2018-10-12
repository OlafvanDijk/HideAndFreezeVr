using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Miniature : MonoBehaviour {

    [SerializeField]
    private int seekerID;
    private PhotonView photonView;
    private Collider voteCollider;
    private VRTK_InteractableObject interactable;
    private bool voted;

    private void Awake()
    {
        this.photonView = GetComponent<PhotonView>();
        photonView.ownerId = PhotonNetwork.player.ID;
    }
    // Use this for initialization
    void Start () {
        if (photonView.isMine)
        {
            interactable = GetComponent<VRTK_InteractableObject>();
            interactable.isGrabbable = true;
        }
	}

    private void Update()
    {
        if (voteCollider != null && !interactable.IsGrabbed())
        {
            VoteLevelAndSeeker vote = voteCollider.GetComponent<VoteLevelAndSeeker>();
            if (vote != null && !voted)
            {
                vote.SendVote(photonView.owner.ID, seekerID);
                voted = true;
                interactable.isGrabbable = false;
                Destroy(interactable.GetComponent<Rigidbody>());
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
