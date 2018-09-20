using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : Photon.PunBehaviour {
    

	private void Start () {
        print("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
	}

    /// <summary>
    /// Called by photon when the user gets connected to the master.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        print("Connected to master.");
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    /// <summary>
    /// Called by photon when the lobby has been joined.
    /// </summary>
    public override void OnJoinedLobby()
    {
        print("Joined Lobby");

        if (!PhotonNetwork.inRoom)
        {
            MainCanvasManager.Instance.ShowLobby();
        }
    }
}
