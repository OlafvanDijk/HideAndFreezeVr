using System.Collections;
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

    [SerializeField]
    private string PlayerPrefabString;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        photonView = GetComponent<PhotonView>();

        PlayerName = "User#" + Random.Range(1000, 9999);

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            if (PhotonNetwork.isMasterClient)
                MasterLoadedGame();
            else
                NonMasterLoadedGame();
        }
    }

    private void MasterLoadedGame()
    {
        //photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
        //photonView.RPC("RPC_LoadGameOthers", PhotonTargets.OthersBuffered);
        NonMasterLoadedGame();
    }

    private void NonMasterLoadedGame()
    {
        //photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", PlayerPrefabString), new Vector3(0, 0, 0), Quaternion.identity, 0);
        obj.GetComponent<LocationDataHolder>().SetPlayer(this.player);
    }
}
