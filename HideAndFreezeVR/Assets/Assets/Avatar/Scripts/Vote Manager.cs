using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : Photon.MonoBehaviour {

    private Dictionary<int, Vote> votes;

	// Use this for initialization
	void Start () {
        votes = new Dictionary<int, Vote>();
    }
	
    public void VoteForLevel(int playerID, string levelName, int seekerID)
    {
        Debug.Log("Player " + playerID + " voted " + levelName + " as level to play with player " + seekerID + " as seeker.");
        try
        {
            votes.Add(playerID, new Vote(seekerID, levelName));
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }
}
