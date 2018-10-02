using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VR_MainCanvasManager : MonoBehaviour{

    public static VR_MainCanvasManager Instance;

    public int test = 4;

    [SerializeField]
    private VR_LobbyCanvas lobbyCanvas;
    public VR_LobbyCanvas getLobbyCanvas()
    {
        return lobbyCanvas;
    }

    private void Awake()
    {
        Instance = this;
        test = 4;
    }

    private void Start()
    {
    }

    /// <summary>
    /// Loads the lobby UI.
    /// </summary>
    public void ShowLobby()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Loads the Room UI.
    /// </summary>
    public void ShowRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
