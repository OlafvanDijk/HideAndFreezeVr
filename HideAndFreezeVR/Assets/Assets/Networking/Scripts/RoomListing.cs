using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour {

    [SerializeField]
    private Text RoomNameText;

    public bool Updated { get; set; }

    public string RoomName;
	
	private void Start () {
        GameObject lobbyCanvasObj = MainCanvasManager.Instance.lobbyCanvas.gameObject;
        if(lobbyCanvasObj == null)
        {
            return;
        }
        LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas>();

        GetComponent<Button>().onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
	}

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void SetRoomNameText(string text)
    {
        RoomName = text;
        RoomNameText.text = RoomName;
    }


}
