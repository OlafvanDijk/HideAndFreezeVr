﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	private void Start () {
        print("Connecting to server..");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
	}

    private void OnConnectedToMaster()
    {
        print("Connected to master.");
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Joined Lobby");
    }
}
