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
        //photonView.ObservedComponents.Add(this);
        DontDestroyOnLoad(this);

    }

    void Update()
    {
        if (photonView.isMine)
        {
            GetLocation();
        }
        else
        {
            SmoothMove();
        }

    }

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

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);
    }

    private void GetLocation()
    {
        if (objectToFollow != null)
        {
            transform.position = Vector3.Lerp(transform.position, objectToFollow.position, 0.25f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, objectToFollow.rotation, 500 * Time.deltaTime);
        }
        else
        {
            //Debug.Log("ObjectToFollow is null");
        }
    }
}
