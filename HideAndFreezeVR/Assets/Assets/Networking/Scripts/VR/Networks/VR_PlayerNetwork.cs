using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VR_PlayerNetwork : MonoBehaviour {
    
    public static VR_PlayerNetwork Instance;

    private PhotonView photonView;

    public string PlayerName { get; private set; }

    private int PlayersInGame = 0;

    [SerializeField]
    private string PlayerPrefabString;
    
    private void Awake()
    {
            Instance = this;

        DontDestroyOnLoad(this.gameObject);

        photonView = GetComponent<PhotonView>();
        photonView.viewID = 999;

            PlayerName = "User#" + Random.Range(1000, 9999);

            PhotonNetwork.sendRate = 60;
            PhotonNetwork.sendRateOnSerialize = 30;

            SceneManager.sceneLoaded += OnSceneFinishedLoading;

    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
    {
        photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
    }


    [PunRPC]
    private void RPC_CreatePlayer()
    {
        Debug.Log("Create Player");
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", PlayerPrefabString), new Vector3(0, 0, 0), Quaternion.identity, 0);
    }
}
