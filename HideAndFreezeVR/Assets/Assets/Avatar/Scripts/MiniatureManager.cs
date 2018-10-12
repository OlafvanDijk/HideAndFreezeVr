using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

//TODONIELS Make this callable from the network
// and Call addMiniature when a new player joins.
public class MiniatureManager : MonoBehaviour {

    [SerializeField]
    [Tooltip("List of placeholders and the Miniatures within them.")]
    private List<PositionAndMiniature> listOfPositionsAndMiniatures = new List<PositionAndMiniature>();
    private List<PlayerIDandAvatarIndex> playersAndAvatars = new List<PlayerIDandAvatarIndex>();

    /// <summary>
    /// Add a Miniature. This is called when a new player joins the game.
    /// </summary>
    /// <param name="avatarIndex">Index of the current avatar.</param>
    /// <param name="outfitIndex">Index of the current outfit.</param>
    /// <param name="playerID">Photon ID of the Player.</param>
    public void AddMiniature(int avatarIndex, int outfitIndex, int playerID)
    {
        foreach (PositionAndMiniature item in listOfPositionsAndMiniatures)
        {
            if (item.miniature == null)
            {
                PlayerIDandAvatarIndex idAndIndex = new PlayerIDandAvatarIndex()
                {
                    playerID = playerID,
                    avatarIndex = avatarIndex
                };
                playersAndAvatars.Add(idAndIndex);
                SpawnAvatar(item.position, playerID, outfitIndex);
                return;
            }
        }
        Debug.Log("No open positions for new Miniatures.");
    }

    /// <summary>
    /// Change clothes of the Miniature.
    /// </summary>
    /// <param name="playerID">Photon ID of the Player.</param>
    /// <param name="outfitIndex">Index of the new outfit.</param>
    public void ChangeClothes(int playerID, int outfitIndex)
    {
        int playerIndex = GetPlayerIndex(playerID);
        GameObject avatar = listOfPositionsAndMiniatures[playerIndex].miniature;
        ChangeOutfitMiniature(avatar.GetComponent<ChangeOutfit>(), playersAndAvatars[playerIndex].avatarIndex, outfitIndex);
    }

    /// <summary>
    /// Changes the outfit of the Miniature.
    /// </summary>
    /// <param name="changeOutfit">ChangeOutfit script of the Miniature.</param>
    /// <param name="avatarIndex">Index of the avatar.</param>
    /// <param name="outfitIndex">Index of the new outfit.</param>
    private void ChangeOutfitMiniature(ChangeOutfit changeOutfit, int avatarIndex, int outfitIndex)
    {
        Outfit chosenOutfit = AvatarManager.Instance.avatarset.getOutfits(avatarIndex).outfits[outfitIndex];
        changeOutfit.ChangeClothes(chosenOutfit.texture);
    }

    /// <summary>
    /// Remove's Miniature from the game.
    /// </summary>
    /// <param name="playerID">Photon ID of the Player.</param>
    public void RemoveMiniature(int playerID)
    {
        PositionAndMiniature positionAndMiniature = listOfPositionsAndMiniatures[GetPlayerIndex(playerID)];
        Destroy(positionAndMiniature.miniature);
        positionAndMiniature.miniature = null;
    }

    /// <summary>
    /// Change Avatar for the Miniature.
    /// </summary>
    /// <param name="playerID">Photon ID of the Player.</param>
    /// <param name="avatarIndex">New avatar's Index.</param>
    public void ChangeAvatar(int playerID, int avatarIndex)
    {
        RemoveMiniature(playerID);
        SpawnAvatar(listOfPositionsAndMiniatures[GetPlayerIndex(playerID)].position, playerID, 0);
    }

    /// <summary>
    /// Spawn the Miniature.
    /// </summary>
    /// <param name="position">Position the Miniature will be spawned at.</param>
    /// <param name="playerID">Photon ID of the Player to spawn a Miniature for.</param>
    /// <param name="outfitIndex">Index of the Outfit of the Miniature and Player.</param>
    private void SpawnAvatar(GameObject position, int playerID, int outfitIndex)
    {
        int avatarIndex = playersAndAvatars.FindIndex(item => item.playerID == playerID);
        PositionAndMiniature positionAndMiniature = listOfPositionsAndMiniatures.Find(item => item.position.Equals(position));
        positionAndMiniature.miniature = Instantiate(AvatarManager.Instance.GetMiniature(avatarIndex), position.transform);
        positionAndMiniature.miniature.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ChangeClothes(playerID, outfitIndex);
    }

    /// <summary>
    /// Removes Miniature of this Player.
    /// </summary>
    /// <param name="playerID">Photon ID of the Player to remove the Miniature for.</param>
    public void RemovePlayer(int playerID)
    {
        RemoveMiniature(playerID);
        playersAndAvatars.RemoveAt(GetPlayerIndex(playerID));
    }

    /// <summary>
    /// Get the index of the player. This index can be used in the lists.
    /// </summary>
    /// <param name="playerID">Photon ID of the Player.</param>
    /// <returns>Index of the player in the lists.</returns>
    private int GetPlayerIndex(int playerID)
    {
        return playersAndAvatars.FindIndex(item => item.playerID == playerID);
    }
}

[Serializable]
public struct PositionAndMiniature
{
    public GameObject position;
    public GameObject miniature;
}


public struct PlayerIDandAvatarIndex
{
    public int playerID;
    public int avatarIndex;
}