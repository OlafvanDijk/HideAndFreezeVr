using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : Photon.PunBehaviour{

    [SerializeField]
    private Text roomName;

    //<Summary> Creates a room that is visible and joinable by other players. </Summary>
    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };

        if (PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default))
        {
            print("Create room succesfully sent");
        }
        else
        {
            print("Create room failed to send");
        }
    }

    //<Summary> Called when a room failed to be created. </Summary>
    //<param name="codeAndMessage"> codeAndMessage[0] is a short ErrorCode and codeAndMessage[1] is a string debug message.</param>
    public override void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create room failed: " + codeAndMessage[1]);
    }

    //<Summary> Called when a room is succesfully created.</Summary>
    public override void OnCreatedRoom()
    {
        print("Room created succesfully");
        MainCanvasManager.Instance.ShowRoom();
    }
}
