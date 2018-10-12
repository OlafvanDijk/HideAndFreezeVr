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

    private void Awake()
    {
        Instance = this;
        this.photonView = GetComponent<PhotonView>();
    }

    public GameObject GetMiniature(int number)
    {
        return avatarset.listOfMiniatures[number];
    }

    /// <summary>
    /// Get the avatar corresponding to the given number with the layers for the head set to visible.
    /// </summary>
    /// <param name="number"> A number the avatar corresponds to. </param>
    /// <returns> An avatar with the VRIK script attached. </returns>
    public VRIK getAvatarWithHead(int number)
    {
        return avatarset.getAvatarWithHead(number);
    }

    /// <summary>
    /// Get the avatar corresponding to the given number with the layers for the head set to invisible.
    /// </summary>
    /// <param name="number"> A number the avatar corresponds to. </param>
    /// <returns> An avatar with the VRIK script attached. </returns>
    public VRIK getAvatarWithoutHead(int number)
    {
        return avatarset.getAvatarWithoutHead(number);
    }

    /// <summary>
    /// Get the outfits corresponding to the given index.
    /// </summary>
    /// <param name="index"> The index of the Outfits location in the avatarset. </param>
    /// <returns> An Outfits scriptableobject containing multiple outfit objects. </returns>
    public Outfits getOutfits(int index)
    {
        return avatarset.getOutfits(index);
    }

    /// <summary>
    /// Get the exact outfit corresponding to the given indexes.
    /// </summary>
    /// <param name="indexes"> At least 2 integers large. First integer corresponds to the Outfits, second corresponds to the Outfit within. </param>
    /// <returns> A specific outfit. </returns>
    public Outfit getOutfit(int[] indexes)
    {
        return avatarset.getOutfits(indexes[0]).outfits[indexes[1]];
    }

    /// <summary>
    /// Called when the player has changed their avatar.
    /// Tells all other players your avatar was changed.
    /// </summary>
    /// <param name="number"> The integer corresponding to their new avatar. </param>
    public void AvatarChanged(int number)
    {
        this.photonView.RPC("PUNRPC_AvatarChanged", PhotonTargets.OthersBuffered, new object[] { number, PhotonNetwork.player });
    }

    /// <summary>
    /// Called when the player changed their outfit.
    /// Tells all other players your outfit was changed.
    /// </summary>
    /// <param name="outfit"> The new Outfit the player is wearing. </param>
    public void OutfitChanged(Outfit outfit)
    {
        int[] number = avatarset.getOutfitIndex(outfit);
        this.photonView.RPC("PUNRPC_OutfitChanged", PhotonTargets.OthersBuffered, new object[] { number, PhotonNetwork.player });
    }

    /// <summary>
    /// Called when the player changed their scale.
    /// Tells all other players your scale has changed.
    /// </summary>
    /// <param name="newScale"> The value needed to set your scale. </param>
    public void ScaleChanged(float newScale)
    {
        this.photonView.RPC("PUNRPC_ScaleChanged", PhotonTargets.OthersBuffered, new object[] { newScale, PhotonNetwork.player });
    }

    /// <summary>
    /// Called when someone has changed their avatar.
    /// Creates a new avatar and gives this to the user it belongs to.
    /// </summary>
    /// <param name="number"> The integer corresponding to the new avatar. </param>
    /// <param name="player"> The player that has changed their avatar. </param>
    [PunRPC]
    public void PUNRPC_AvatarChanged(int number, PhotonPlayer player)
    {
        VR_PlayerNetwork.Instance.getPlayer(player).SetAvatar(getAvatarWithHead(number));
    }

    /// <summary>
    /// Called when someone has changed their outfit.
    /// Alters the outfit of the user it belongs to.
    /// </summary>
    /// <param name="number"> The 2 integers needed to find the new outfit. </param>
    /// <param name="player"> The player that has changed their outfit. </param>
    [PunRPC]
    public void PUNRPC_OutfitChanged(int[] number, PhotonPlayer player)
    {
        VR_PlayerNetwork.Instance.getPlayer(player).SetOutfit(number);
    }

    /// <summary>
    /// Called when someone has changed their scale.
    /// Alters the scale of the avatar this belongs to.
    /// </summary>
    /// <param name="newScale"> The new scale value. </param>
    /// <param name="player"> The player that has changed their scale. </param>
    [PunRPC]
    public void PUNRPC_ScaleChanged(float newScale, PhotonPlayer player)
    {
        VR_PlayerNetwork.Instance.getPlayer(player).GetComponent<LocationDataHolder>().SetScale(newScale);
    }

}
