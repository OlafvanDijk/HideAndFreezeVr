using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VR_PlayerListing : MonoBehaviour {

	
    public PhotonPlayer photonPlayer { get; private set; }

    [SerializeField]
    private Text playerName;
    
    public Text getPlayerName()
    {
        return playerName;
    }

    /// <summary>
    /// Sets the photonPlayer object and changes the text of the object to the player's name.
    /// </summary>
    /// <param name="photonPlayer"></param>
    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        this.photonPlayer = photonPlayer;
        playerName.text = photonPlayer.NickName;
    }
}
