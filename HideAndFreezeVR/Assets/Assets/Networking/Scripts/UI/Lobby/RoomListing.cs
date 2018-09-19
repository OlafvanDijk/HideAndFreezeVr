using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour {

    [SerializeField]
    private Text RoomNameText;

    public bool Updated { get; set; }

    public string RoomName { get; set; }
	
	private void Start () {
        Setup();    
	}

    //<Summary> Sets up the required data at the start of the object's existence. </Summary>
    private void Setup()
    {
        GameObject lobbyCanvasObj = MainCanvasManager.Instance.getLobbyCanvas().gameObject;
        if (lobbyCanvasObj == null)
        {
            return;
        }
        LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas>();

        GetComponent<Button>().onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
    }

    //<Summary> IMPORTANT; if this object is destroyed, all of it's listeners must be removed aswell. </Summary>
    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    //<Summary> Sets the roomname both in data and on the UI. </Summary>
    public void SetRoomNameText(string text)
    {
        RoomName = text;
        RoomNameText.text = RoomName;
    }


}
