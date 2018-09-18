using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasManager : MonoBehaviour {

    public static MainCanvasManager Instance;

    public LobbyCanvas lobbyCanvas;

    private void Awake()
    {
        Instance = this;
    }

   
}
