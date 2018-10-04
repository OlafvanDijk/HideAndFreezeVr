using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class LocationDataHolder : Photon.MonoBehaviour {

    public Avatars avatarSet;

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
            GameObject avatar = Instantiate(avatarSet.getAvatarWithHead(0));
            avatar.transform.SetParent(this.gameObject.transform);
            SetAvatar(avatar);
        }
        this.gameObject.transform.position += new Vector3(0, 0.2f, 0);

    }

    /// <summary>
    /// Sets the avatar and connects the avatar to the scripts' head and hands.
    /// </summary>
    /// <param name="Avatar">The avatar to bind</param>
    public void SetAvatar(GameObject Avatar)
    {
        this.Avatar = Avatar;
        this.Avatar.GetComponent<VRIK>().solver.spine.headTarget = head.transform;
        this.Avatar.GetComponent<VRIK>().solver.leftArm.target = leftHand.transform;
        this.Avatar.GetComponent<VRIK>().solver.rightArm.target = rightHand.transform;
    }

    /// <summary>
    /// Sets the LocationDataPlayer that this object should follow.
    /// </summary>
    /// <param name="player">The LocationDataPlayer this object should follow.</param>
    public void SetPlayer(LocationDataPlayer player)
    {
        this.player = player;

        leftHand.objectToFollow = player.leftHand;
        rightHand.objectToFollow = player.rightHand;
        head.objectToFollow = player.head;
    }

    /// <summary>
    /// Tells this object's bound children to send data through the stream.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        leftHand.OnPhotonSerializeView(stream, info);
        rightHand.OnPhotonSerializeView(stream, info);
        head.OnPhotonSerializeView(stream, info);
    }
 }
