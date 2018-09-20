using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainCanvasManager : MainCanvasManager {



    

    [SerializeField]
    private CurrentRoomCanvas currentRoomCanvas;
    public CurrentRoomCanvas getCurrentRoomCanvas()
    {
        return currentRoomCanvas;
    }

    private void Awake()
    {
        //Instance = this;
    }

    /// <summary>
    /// Loads the lobby UI.
    /// </summary>
    public override void ShowLobby()
    {
        getLobbyCanvas().gameObject.SetActive(true);
        currentRoomCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Loads the Room UI.
    /// </summary>
    public override void ShowRoom()
    {
        getLobbyCanvas().gameObject.SetActive(false);
        currentRoomCanvas.gameObject.SetActive(true);
    }

   
}
