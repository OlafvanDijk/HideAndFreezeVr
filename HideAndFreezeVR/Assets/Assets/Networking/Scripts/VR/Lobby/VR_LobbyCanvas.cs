using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_LobbyCanvas : MonoBehaviour {

    [SerializeField]
    private VR_RoomLayoutGroup roomLayoutGroup;

    /// <summary>
    /// Should be called from a button.
    /// Connects the user to the given room name and loads the room UI.
    /// </summary>
    /// <param name="roomName">The name of the room the user should join.</param>
	public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            VR_MainCanvasManager.Instance.ShowRoom();
        }
        else
        {
            print("Join room failed");
        }
    }
}
