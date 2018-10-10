using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAndSeekerSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void VoteForLevel(int playerID, string level)
    {
        Debug.Log("Player " + playerID + " voted " + level + " as level to play.");
    }

    public void VoteForSeeker(int playerID, int seekerID)
    {
        Debug.Log("Player " + playerID + " voted for " + seekerID + " to play as seeker.");
    }
}
