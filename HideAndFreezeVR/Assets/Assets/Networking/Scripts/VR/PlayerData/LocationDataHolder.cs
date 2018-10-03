using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDataHolder : Photon.MonoBehaviour {

    [SerializeField]
    private LocationData leftHand;
    [SerializeField]
    private LocationData rightHand;
    [SerializeField]
    private LocationData head;

    private LocationDataPlayer player;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        //this.GetComponent<PhotonView>().ObservedComponents.Clear();
    }

    public void SetPlayer(LocationDataPlayer player)
    {
        this.player = player;

        leftHand.objectToFollow = player.leftHand;
        rightHand.objectToFollow = player.rightHand;
        head.objectToFollow = player.head;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        leftHand.OnPhotonSerializeView(stream, info);
        rightHand.OnPhotonSerializeView(stream, info);
        head.OnPhotonSerializeView(stream, info);
    }
 }
