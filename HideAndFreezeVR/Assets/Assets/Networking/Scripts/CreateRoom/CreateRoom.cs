using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : Photon.PunBehaviour{

    [SerializeField]
    private Text roomName;
    
    /// <summary>
    /// Should be called from a button
    /// Creates a room that is visible and joinable by other players.
    /// </summary>
    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 6 };

        if (PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default))
        {
            print("Create room succesfully sent");
        }
        else
        {
            print("Create room failed to send");
        }
    }
    
    /// <summary>
    /// Called by photon when a room failed to be created.
    /// </summary>
    /// <param name="codeAndMessage">codeAndMessage[0] is a short ErrorCode and codeAndMessage[1] is a string debug message.</param>
    public override void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create room failed: " + codeAndMessage[1]);
    }

    /// <summary>
    /// Called by photon when a room is succesfully created.
    /// </summary>
    public override void OnCreatedRoom()
    {
        print("Room created succesfully");
        MainCanvasManager.Instance.ShowRoom();
    }
}
