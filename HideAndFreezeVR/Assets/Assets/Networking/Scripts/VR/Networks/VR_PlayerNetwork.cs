﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VR_PlayerNetwork : MonoBehaviour {
    
    public static VR_PlayerNetwork Instance;

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

    public void AddOtherPlayer(LocationDataHolder holder)
    {
        otherPlayers.Add(holder);
    }

    public void RemoveOtherPlayer(PhotonPlayer player)
    {
        foreach(LocationDataHolder holder in otherPlayers)
        {
            if(holder.GetComponent<PhotonView>().owner == player)
            {
                otherPlayers.Remove(holder);
            }
        }
    }

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
