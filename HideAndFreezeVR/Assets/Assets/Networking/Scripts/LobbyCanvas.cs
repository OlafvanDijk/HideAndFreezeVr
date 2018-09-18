using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {

    [SerializeField]
    private RoomLayoutGroup roomLayoutGroup;

	public void OnClickJoinRoom(string roomName)
    {
        Debug.Log("lobbyclick");
    }
}
