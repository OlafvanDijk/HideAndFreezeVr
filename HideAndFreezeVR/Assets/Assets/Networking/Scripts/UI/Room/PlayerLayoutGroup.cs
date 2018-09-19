using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup : Photon.PunBehaviour {

    [SerializeField]
    private GameObject playerListingPrefab;

    private List<PlayerListing> playerListings = new List<PlayerListing>();

    public GameObject getPlayerListingPrefab()
    {
        return playerListingPrefab;
    }

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

    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        int index = playerListings.FindIndex(x => x.photonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(playerListings[index].gameObject);
            playerListings.RemoveAt(index);
        }
    }

    public void OnClickRoomState()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }

        PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
        PhotonNetwork.room.IsVisible = !PhotonNetwork.room.IsVisible;
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
