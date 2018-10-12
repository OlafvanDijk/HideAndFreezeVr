using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : Photon.MonoBehaviour
{

    private List<int> playerVoted;
    private Dictionary<int, int> seekerVoted;
    private Dictionary<string, int> levelVoted;

    // Use this for initialization
    void Start()
    {
        playerVoted = new List<int>();
        seekerVoted = new Dictionary<int, int>();
        levelVoted = new Dictionary<string, int>();
    }

    public void VoteForLevel(int playerID, string levelName, int seekerID)
    {
        if (!playerVoted.Contains(playerID))
        {
            playerVoted.Add(playerID);
            if (seekerVoted.ContainsKey(seekerID))
            {
                seekerVoted[seekerID] += 1;
            }
            else
            {
                seekerVoted.Add(seekerID, 1);
            }
            if (levelVoted.ContainsKey(levelName))
            {
                levelVoted[levelName] += 1;
            }
            else
            {
                levelVoted.Add(levelName, 1);
            }
        }
        Debug.Log("Player " + playerID + " voted " + levelName + " as level to play with player " + seekerID + " as seeker.");
        if (playerVoted.Count == PhotonNetwork.room.PlayerCount)
        {
            EndOfVoting();
        }
    }

    private void EndOfVoting()
    {
        KeyValuePair<int, int> seeker = new KeyValuePair<int, int>(-1, -1);
        foreach (KeyValuePair<int, int> entry in seekerVoted)
        {
            if (entry.Value > seeker.Value)
            {
                seeker = entry;
            }
            else if (entry.Value == seeker.Value)
            {
                if (Random.Range(0, 1) == 1)
                {
                    seeker = entry;
                }
            }
        }
        if (seeker.Key == -1)
        {
            seeker = new KeyValuePair<int, int>(PhotonNetwork.playerList[Random.Range(0, PhotonNetwork.room.PlayerCount)].ID, 0);
        }
        Debug.Log("Seeker is " + seeker.Key);

        KeyValuePair<string, int> level = new KeyValuePair<string, int>("no level", -1);
        foreach (KeyValuePair<string, int> entry in levelVoted)
        {
            if (entry.Value > level.Value)
            {
                level = entry;
            }
            else if (entry.Value == level.Value)
            {
                if (Random.Range(0, 1) == 1)
                {
                    level = entry;
                }
            }
        }
        if (level.Key.Equals("no level"))
        {
            level = new KeyValuePair<string, int>("level 1", 0);
        }

        Debug.Log("Level is " + level.Key);
    }
}
