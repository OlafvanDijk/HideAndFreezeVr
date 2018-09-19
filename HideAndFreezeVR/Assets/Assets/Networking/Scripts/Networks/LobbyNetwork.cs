using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : Photon.PunBehaviour {

	// Use this for initialization
	private void Start () {
        print("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
	}

    public override void OnConnectedToMaster()
    {
        print("Connected to master.");
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        print("Joined Lobby");

        if (!PhotonNetwork.inRoom)
        {
            MainCanvasManager.Instance.ShowLobby();
        }
    }
}
