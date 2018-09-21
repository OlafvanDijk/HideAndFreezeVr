using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePhotonView : Photon.MonoBehaviour {

    private PhotonView photonView;
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;

    private void Update()
    {
        if (photonView != null)
        {
            if (photonView.isMine)
            {

            }
            else
            {
                SmoothMove();
            }
        }
    }

    public void AddPhotonView(int index)
    {
        photonView = this.gameObject.AddComponent<PhotonView>();
        int playerNumber = whichPlayerAmI() * 10;
        photonView.viewID = playerNumber + index;
        photonView.ObservedComponents = new List<Component>();
        photonView.ObservedComponents.Add(this.gameObject.transform);
    }

    private int whichPlayerAmI()
    {
        int x = 1;
        foreach(PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            if (photonPlayer.IsLocal)
            {
                return x;
            }
            x += 1;
        }
        return -1;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 500 * Time.deltaTime);
    }


}
