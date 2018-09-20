using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLayoutGroup : Photon.PunBehaviour {

    [SerializeField]
    private GameObject playerListingPrefab;
    public GameObject getPlayerListingPrefab()
    {
        return playerListingPrefab;
    }

    private List<PlayerListing> playerListings = new List<PlayerListing>();
    public List<PlayerListing> getPlayerListings()
    {
        return playerListings;
    }

    /// <summary>
    /// Called by photon whenever the master client is switched.
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// Called by photon whenever you join a room
    /// </summary>
    public override void OnJoinedRoom()
    {
        foreach(PlayerListing playerListing in playerListings)
        {
            Destroy(playerListing.gameObject);
        }
        playerListings.Clear();

        MainCanvasManager.Instance.ShowRoom();

        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for(int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }
    
    /// <summary>
    /// Called by photon when a player joins the room.
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        PlayerJoinedRoom(newPlayer);
    }
    
    /// <summary>
    /// Called by photon when a player leaves the room
    /// </summary>
    /// <param name="photonPlayer"></param>
    public override void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="photonPlayer"></param>
    private void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        if(photonPlayer == null)
        {
            return;
        }

        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(playerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPlayer(photonPlayer);

        playerListings.Add(playerListing);
    }

    /// <summary>
    /// Handles what should happen when an other player leaves the room.
    /// </summary>
    /// <param name="photonPlayer"> The player that left the room. </param>
    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        int index = playerListings.FindIndex(x => x.photonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(playerListings[index].gameObject);
            playerListings.RemoveAt(index);
        }
    }

    /// <summary>
    /// Should be called from a button.
    /// Changes the roomstate to open or close, visible and not visible.
    /// </summary>
    public void OnClickRoomState(Text StateText)
    {
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }

        PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
        PhotonNetwork.room.IsVisible = !PhotonNetwork.room.IsVisible;

        switch (PhotonNetwork.room.IsOpen)
        {
            case true:
                StateText.text = "Room State \n \n Room is open.";
                break;

            case false:
                StateText.text = "Room State \n \n Room is closed.";
                break;
        }

    }

    /// <summary>
    /// Should be called from a button.
    /// Leave the room.
    /// </summary>
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
