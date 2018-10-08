﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;


public class LocationDataHolder : Photon.MonoBehaviour  {

    [SerializeField] private LocationData leftHand;
    [SerializeField] private Transform leftHandOffset;
    [SerializeField] private LocationData rightHand;
    [SerializeField] private Transform rightHandOffset;
    [SerializeField] private LocationData head;
    [SerializeField] private Transform headOffset;

    private VRIK Avatar;

    public Outfit outfit;

    private LocationDataPlayer player;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (!GetComponentInParent<PhotonView>().isMine)
        {
            VRIK avatar = Instantiate(AvatarManager.Instance.getAvatarWithHead(0));
            SetAvatar(avatar);
        }
        this.gameObject.transform.position += new Vector3(0, 0.2f, 0);
        VR_PlayerNetwork.Instance.AddOtherPlayer(this);

    }

    private void Start()
    {

    }

    /// <summary>
    /// Sets the avatar and connects the avatar to the scripts' head and hands.
    /// </summary>
    /// <param name="Avatar">The avatar to bind</param>
    public void SetAvatar(VRIK Avatar)
    {
        if (this.Avatar != null)
        {
            Destroy(this.Avatar.gameObject);
        }
        this.Avatar = Avatar;
        this.Avatar.transform.SetParent(this.gameObject.transform);
        this.Avatar.solver.spine.headTarget = headOffset;
        this.Avatar.solver.leftArm.target = leftHandOffset;
        this.Avatar.solver.rightArm.target = rightHandOffset;
    }

    public void SetOutfit(int[] number)
    {
        Outfit _outfit = AvatarManager.Instance.getOutfit(number);
        this.outfit = _outfit;
        this.Avatar.GetComponent<ChangeOutfit>().ChangeClothes(_outfit.texture);
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
