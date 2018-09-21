﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomListing : MonoBehaviour {

    [SerializeField]
    private Text RoomNameText;

    public bool Updated { get; set; }

    public string RoomName { get; set; }
	
	private void Start () {
        Setup();    
	}
    
    /// <summary>
    /// Sets up the required data at the start of the object's existence.
    /// </summary>
    private void Setup()
    {
        GameObject lobbyCanvasObj = UI_MainCanvasManager.Instance.getLobbyCanvas().gameObject;
        if (lobbyCanvasObj == null)
        {
            return;
        }
        UI_LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<UI_LobbyCanvas>();

        GetComponent<Button>().onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
    }
    
    /// <summary>
    /// IMPORTANT; if this object is destroyed, all of it's listeners must be removed aswell.
    /// </summary>
    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Sets the room name on the UI and saves it.
    /// </summary>
    /// <param name="text"> The given room name. </param>
    public void SetRoomNameText(string text)
    {
        RoomName = text;
        RoomNameText.text = RoomName;
    }


}