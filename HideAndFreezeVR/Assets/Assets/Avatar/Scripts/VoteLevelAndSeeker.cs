using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteLevelAndSeeker : MonoBehaviour {
    
    [SerializeField]
    private VoteManager levelAndSeekerSelect;
    [SerializeField]
    private int seekerID;
    [SerializeField]
    private string levelName;

    public void SendVote(int playerID)
    {
        levelAndSeekerSelect.VoteForLevel(playerID, levelName, seekerID);
    }
}
