using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vote {

    public int seekerID { get; private set; }
    public string levelName { get; private set; }

    public Vote(int seekerID, string levelName)
    {
        this.seekerID = seekerID;
        this.levelName = levelName;
    }
}
