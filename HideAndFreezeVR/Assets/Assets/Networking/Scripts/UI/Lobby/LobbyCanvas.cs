using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {

    [SerializeField]
    private RoomLayoutGroup roomLayoutGroup;

    // <Summary>Called when a player joins a room </Summary>
	public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            MainCanvasManager.Instance.ShowRoom();
        }
        else
        {
            print("Join room failed");
        }
    }
}
