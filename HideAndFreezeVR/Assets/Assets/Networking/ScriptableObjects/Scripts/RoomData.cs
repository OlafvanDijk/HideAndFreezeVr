using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "RoomData", order = 1)]
public class RoomData : ScriptableObject {

    public List<PhotonPlayer> players;
}
