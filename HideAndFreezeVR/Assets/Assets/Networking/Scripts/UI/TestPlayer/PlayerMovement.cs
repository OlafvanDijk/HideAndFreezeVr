﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Photon.MonoBehaviour {

    private PhotonView photonView;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }


    void Update () {
        if (photonView.isMine) {
            CheckInput();
        }
        else
        {
            SmoothMove();
        }

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
            targetPosition = (Vector3)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);
    }



    //Not important for networking
    private void CheckInput()
    {
        float moveSpeed = 10f;
        float rotateSpeed = 200f;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.position += transform.forward * (vertical * moveSpeed * Time.deltaTime);
        transform.Rotate(new Vector3(0, horizontal * rotateSpeed * Time.deltaTime));
    }

}