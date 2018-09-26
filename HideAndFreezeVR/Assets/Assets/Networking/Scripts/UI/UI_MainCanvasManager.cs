using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainCanvasManager : MonoBehaviour {

    public static UI_MainCanvasManager Instance;

    [SerializeField]
    private UI_LobbyCanvas lobbyCanvas;
    public UI_LobbyCanvas getLobbyCanvas()
    {
        return lobbyCanvas;
    }

    [SerializeField]
    private UI_CurrentRoomCanvas currentRoomCanvas;
    public UI_CurrentRoomCanvas getCurrentRoomCanvas()
    {
        return currentRoomCanvas;
    }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Loads the lobby UI.
    /// </summary>
    public void ShowLobby()
    {
        getLobbyCanvas().gameObject.SetActive(true);
        currentRoomCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Loads the Room UI.
    /// </summary>
    public void ShowRoom()
    {
        getLobbyCanvas().gameObject.SetActive(false);
        currentRoomCanvas.gameObject.SetActive(true);
    }

   
}
