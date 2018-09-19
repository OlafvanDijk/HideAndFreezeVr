using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour {

	
    public PhotonPlayer photonPlayer { get; private set; }

    [SerializeField]
    private Text playerName;

    public Text getPlayerName()
    {
        return playerName;
    }

    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        this.photonPlayer = photonPlayer;
        playerName.text = photonPlayer.NickName;
    }
}
