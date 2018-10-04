using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_CurrentRoomCanvas : MonoBehaviour {
    
    /// <summary>
    /// Called by a button. loads a new scene.
    /// Only the masterclient can call this.
    /// </summary>
    public void OnClickStart()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }
        
        PhotonNetwork.LoadLevel(2);
    }
    

}
