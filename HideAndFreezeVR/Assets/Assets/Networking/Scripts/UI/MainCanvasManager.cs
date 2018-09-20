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

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Loads the lobby UI.
    /// </summary>
    public virtual void ShowLobby()
    {
    }

    /// <summary>
    /// Loads the Room UI.
    /// </summary>
    public virtual void ShowRoom()
    {
    }
}
