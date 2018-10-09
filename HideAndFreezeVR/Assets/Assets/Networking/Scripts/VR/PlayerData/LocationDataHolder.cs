using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class LocationDataHolder : Photon.MonoBehaviour {

    [SerializeField]
    private LocationData leftHand;
    [SerializeField]
    private LocationData rightHand;
    [SerializeField]
    private LocationData head;

    private GameObject Avatar;

    private LocationDataPlayer player;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (!GetComponentInParent<PhotonView>().isMine)
        {
            GameObject avatar = Instantiate(AvatarManager.Instance.GetRandomAvatar());
            avatar.transform.SetParent(this.gameObject.transform);
            SetAvatar(avatar);
        }

    }

    public void SetAvatar(GameObject Avatar)
    {
        this.Avatar = Avatar;
        this.Avatar.GetComponent<VRIK>().solver.spine.headTarget = head.transform;
        this.Avatar.GetComponent<VRIK>().solver.leftArm.target = leftHand.transform;
        this.Avatar.GetComponent<VRIK>().solver.rightArm.target = rightHand.transform;
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
