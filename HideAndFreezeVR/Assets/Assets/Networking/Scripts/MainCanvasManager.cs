using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasManager : MonoBehaviour {

    public static MainCanvasManager Instance;

    [SerializeField]
    private LobbyCanvas lobbyCanvas;
    public LobbyCanvas getLobbyCanvas()
    {
        return lobbyCanvas;
    }

    [SerializeField]
    private CurrentRoomCanvas currentRoomCanvas;
    public CurrentRoomCanvas getCurrentRoomCanvas()
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
        lobbyCanvas.gameObject.SetActive(true);
        currentRoomCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Loads the Room UI.
    /// </summary>
    public void ShowRoom()
    {
        lobbyCanvas.gameObject.SetActive(false);
        currentRoomCanvas.gameObject.SetActive(true);
    }

   
}
