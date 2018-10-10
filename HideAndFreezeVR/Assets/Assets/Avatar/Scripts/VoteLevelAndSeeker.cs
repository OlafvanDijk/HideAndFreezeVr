using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteLevelAndSeeker : MonoBehaviour {
    
    [SerializeField]
    private LevelAndSeekerSelect levelAndSeekerSelect;
    [SerializeField]
    private bool levelOrSeeker;
    [SerializeField]
    private int seekerID;
    [SerializeField]
    private string levelName;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendVote(int playerID)
    {
        if (levelOrSeeker)
        {
            levelAndSeekerSelect.VoteForLevel(playerID, levelName);
        }
        else
        {
            levelAndSeekerSelect.VoteForSeeker(playerID, seekerID);
        }
    }
}
