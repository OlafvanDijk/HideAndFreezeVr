using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_MainCanvasManager : MainCanvasManager{

    /// <summary>
    /// Loads the lobby UI.
    /// </summary>
    public override void ShowLobby()
    {
    }

    /// <summary>
    /// Loads the Room UI.
    /// </summary>
    public override void ShowRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
