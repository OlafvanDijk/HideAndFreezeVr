using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationData : Photon.MonoBehaviour {

    private PhotonView photonView;

    public Transform objectToFollow { get; set; }

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Awake()
    {
        photonView = GetComponentInParent<PhotonView>();
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (photonView.isMine) //Check if this belongs to the player or one of the other players.
        {
            GetLocation();
        }
        else
        {
            SmoothMove();
        }

    }

    /// <summary>
    /// Sends and receives data to and from other players.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            targetPosition = (Vector3)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// Move the transform this script is connected to to the targetposition and rotation.
    /// </summary>
    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);
    }

    /// <summary>
    /// Move the transform this script is connected to to the object it is told to follow.
    /// </summary>
    private void GetLocation()
    {
        if (objectToFollow != null)
        {
            transform.position = Vector3.Lerp(transform.position, objectToFollow.position, 0.25f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, objectToFollow.rotation, 500 * Time.deltaTime);
        }
    }

    /// <summary>
    /// TODONIELS TODOOLAF Delete when not needed anymore
    /// </summary>
    public void test(bool owner)
    {
        targetPosition = this.transform.position;
        targetRotation = this.transform.rotation;
        if (owner)
        {
            //photonView.TransferOwnership(PhotonNetwork.player);
        }
    }
}
