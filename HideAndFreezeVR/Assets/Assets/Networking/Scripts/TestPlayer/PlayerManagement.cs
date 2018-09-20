using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour {

    public static PlayerManagement Instance;
    private PhotonView photonView;

    private List<PlayerStats> playerstats = new List<PlayerStats>();

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
    }

    public void AddPlayerStats(PhotonPlayer photonPlayer)
    {
        int index = playerstats.FindIndex(x => x.photonPlayer == photonPlayer);
        if(index == -1)
        {
            playerstats.Add(new PlayerStats(photonPlayer, 30));
        }

    }

    public void ModifyHealth(PhotonPlayer photonPlayer, int value)
    {
        int index = playerstats.FindIndex(x => x.photonPlayer == photonPlayer);
        if(index == -1)
        {
            playerstats[index].health += value;
            PlayerNetwork.Instance.NewHealth(photonPlayer, playerstats[index].health);
        }
    }

}

public class PlayerStats
{
    public PlayerStats(PhotonPlayer photonPlayer, int health)
    {
        this.photonPlayer = photonPlayer;
        this.health = health;
    }

    public readonly PhotonPlayer photonPlayer;
    public int health;
}
