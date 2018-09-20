using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_RoomLayoutGroup : Photon.PunBehaviour {

    [SerializeField]
    private VR_RoomListing RoomListingPrefab;

    private List<VR_RoomListing> roomlistingButtons = new List<VR_RoomListing>();


    /// <summary>
    /// Called by photo when the room list was updated.
    /// </summary>
    public override void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in rooms)
        {
            RoomReceived(room);
        }

        RemoveOldRooms();
    }

    /// <summary>
    /// Place the new room in the list of rooms.
    /// </summary>
    /// <param name="room"> The room to be added. </param>
    private void RoomReceived(RoomInfo room)
    {
        int index = roomlistingButtons.FindIndex(x => x.RoomName == room.Name);
        if(index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab.gameObject);
                roomListingObj.transform.SetParent(transform, false);

                VR_RoomListing roomListing = roomListingObj.GetComponent<VR_RoomListing>();
                roomlistingButtons.Add(roomListing);

                index = (roomlistingButtons.Count - 1);
            }
        }
        if(index != -1)
        {
            VR_RoomListing roomListing = roomlistingButtons[index];
            roomListing.SetRoomNameText(room.Name);
            roomListing.Updated = true;
        }
    }

    /// <summary>
    /// Removes all old rooms that are not accessible any more.
    /// </summary>
    private void RemoveOldRooms()
    {
        List<VR_RoomListing> removeRooms = new List<VR_RoomListing>();
        
        foreach(VR_RoomListing roomListing in roomlistingButtons)
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
        foreach(VR_RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;
            roomlistingButtons.Remove(roomListing);
            Destroy(roomListingObj);
        }
    }

}
