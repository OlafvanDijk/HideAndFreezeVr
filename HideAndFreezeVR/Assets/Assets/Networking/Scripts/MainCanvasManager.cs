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

    public void ShowLobby()
    {
        lobbyCanvas.gameObject.SetActive(true);
        currentRoomCanvas.gameObject.SetActive(false);
    }

    public void ShowRoom()
    {
        lobbyCanvas.gameObject.SetActive(false);
        currentRoomCanvas.gameObject.SetActive(true);
    }

   
}
