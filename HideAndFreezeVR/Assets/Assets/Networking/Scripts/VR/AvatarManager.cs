using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Easily accesibly database for avatars.
/// </summary>
public class AvatarManager : Photon.MonoBehaviour {

    public static AvatarManager Instance;

    public Avatars avatarset;
    private PhotonView photonView;

    public Outfits test;
    public Outfit test2;

    private void Awake()
    {
        Instance = this;
        this.photonView = GetComponent<PhotonView>();
    }

    public VRIK getAvatarWithHead(int number)
    {
        return avatarset.getAvatarWithHead(number);
    }

    public VRIK getAvatarWithoutHead(int number)
    {
        return avatarset.getAvatarWithoutHead(number);
    }

    public Outfits getOutfits(int index)
    {
        return avatarset.getOutfits(index);
    }

    public Outfit getOutfit(int[] indexes)
    {
        return avatarset.getOutfits(indexes[0]).outfits[indexes[1]];
    }

    private int getAvatarNumber(VRIK avatar)
    {
        int number = 0;
        foreach (VRIK Avatar in avatarset.listOfAvatars)
        {
            if(avatar == Avatar)
            {
                break;
            }
            number++;
        }
        return number;
    }

    public void AvatarChanged(int number)
    {
        this.photonView.RPC("PUNRPC_AvatarChanged", PhotonTargets.OthersBuffered, new object[] { number, PhotonNetwork.player });
    }

    public void OutfitChanged(Outfit outfit)
    {
        int[] number = avatarset.getOutfitIndex(outfit);
        this.photonView.RPC("PUNRPC_OutfitChanged", PhotonTargets.OthersBuffered, new object[] { number, PhotonNetwork.player });
    }


    [PunRPC]
    public void PUNRPC_AvatarChanged(int number, PhotonPlayer player)
    {
        VRIK avatar = Instantiate(getAvatarWithHead(number));
        VR_PlayerNetwork.Instance.getPlayer(player).SetAvatar(avatar);
    }

    [PunRPC]
    public void PUNRPC_OutfitChanged(int[] number, PhotonPlayer player)
    {
        VR_PlayerNetwork.Instance.getPlayer(player).SetOutfit(number);
    }

}
