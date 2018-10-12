using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteLevelAndSeeker : MonoBehaviour {
    
    [SerializeField]
    private VoteManager voteManager;
    [SerializeField]
    private string levelName;

    public void SendVote(int playerID, int seekerID)
    {
        voteManager.VoteForLevel(playerID, levelName, seekerID);
    }
}
