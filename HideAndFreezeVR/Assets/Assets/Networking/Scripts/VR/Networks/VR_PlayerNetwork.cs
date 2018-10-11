using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VR_PlayerNetwork : Photon.PunBehaviour {
    
    public static VR_PlayerNetwork Instance;

    public PlayerData playerData;

    private PhotonView photonView;
    public LocationDataPlayer player { get; set; }
    public string PlayerName { get; private set; }
    private int PlayersInGame = 0;

    private List<LocationDataHolder> otherPlayers;

    [SerializeField]
    private string PlayerPrefabString;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        otherPlayers = new List<LocationDataHolder>();
        photonView = GetComponent<PhotonView>();
        photonView.viewID = 999;

        PlayerName = "User#" + Random.Range(1000, 9999);

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    /// <summary>
    /// Called when the scene has finished loading.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(waitForPlayer(scene));
    }

    /// <summary>
    /// Gets called when a player disconnects from the room.
    /// </summary>
    /// <param name="photonPlayer"></param>
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        RemoveOtherPlayer(photonPlayer);
    }

    /// <summary>
    /// Called when the player leaves a room. Redirects the player back to the lobby.
    /// </summary>
    public override void OnLeftRoom()
    {
        otherPlayers.Clear();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Waits for the player to have correctly joined the room before doing anything.
    /// </summary>
    /// <param name="scene">The scene that is being loaded.</param>
    private IEnumerator waitForPlayer(Scene scene)
    {
        this.player = null;
        yield return new WaitUntil(() => this.player != null);
        if (scene.name == "VR_Room")
        {
            SpawnPlayer();
        }
    }
    
    /// <summary>
    /// Spawns the player object and sets the player to the LocationHolder.
    /// </summary>
    private void SpawnPlayer()
    {
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", PlayerPrefabString), new Vector3(0, 0, 0), Quaternion.identity, 0);
        obj.GetComponent<LocationDataHolder>().SetPlayer(this.player);
    }

    /// <summary>
    /// Adds a new player to the list of players.
    /// </summary>
    /// <param name="holder"> The new player. </param>
    public void AddOtherPlayer(LocationDataHolder holder)
    {
        otherPlayers.Add(holder);
    }

    /// <summary>
    /// Removes a player from the list of players.
    /// </summary>
    /// <param name="player"></param>
    public void RemoveOtherPlayer(PhotonPlayer player)
    {
        for(int i = otherPlayers.Count-1; i > -1; i--)
        {
            Debug.Log("Other players: " + otherPlayers[i]);
            if (otherPlayers[i].gameObject.GetComponent<PhotonView>().owner == player || otherPlayers[i].gameObject.GetComponent<PhotonView>().owner == null)
            {
                Debug.Log(player.NickName + " has left.");
                otherPlayers.RemoveAt(i);
            }
        }

    }

    /// <summary>
    /// Gets the LocationDataHolder that belongs to the given player.
    /// </summary>
    /// <param name="player"> The player you want an object from. </param>
    /// <returns> A LocationDataHolder object. </returns>
    public LocationDataHolder getPlayer(PhotonPlayer player)
    {
        foreach (LocationDataHolder holder in otherPlayers)
        {
            if (holder.GetComponent<PhotonView>().owner == player)
            {
                return holder;
            }
        }
        return null;
    }
}
