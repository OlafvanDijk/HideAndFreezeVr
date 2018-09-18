using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour {

    [SerializeField]
    private RoomListing RoomListingPrefab;

    private List<RoomListing> roomlistingButtons = new List<RoomListing>();

    private void OnReceivedRoomListUpdate()
    {
        
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in rooms)
        {
            RoomReceived(room);
        }

        RemoveOldRooms();
    }

    private void RoomReceived(RoomInfo room)
    {
        int index = roomlistingButtons.FindIndex(x => x.RoomName == room.Name);
        if(index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab.gameObject);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                roomlistingButtons.Add(roomListing);

                index = (roomlistingButtons.Count - 1);
            }
        }
        if(index != -1)
        {
            RoomListing roomListing = roomlistingButtons[index];
            roomListing.SetRoomNameText(room.Name);
            roomListing.Updated = true;
        }
    }

    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();
        
        foreach(RoomListing roomListing in roomlistingButtons)
        {
            if (!roomListing.Updated)
            {
                removeRooms.Add(roomListing);
            }
            else
            {
                roomListing.Updated = false;
            }
        }
        foreach(RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;
            roomlistingButtons.Remove(roomListing);
            Destroy(roomListingObj);
        }
    }

}
