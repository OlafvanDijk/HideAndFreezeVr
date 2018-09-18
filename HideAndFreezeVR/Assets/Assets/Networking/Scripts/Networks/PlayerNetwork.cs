﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork Instance;
    public string PlayerName { get; private set; }

    private void Awake()
    {
        Instance = this;
        PlayerName = "User#" + Random.Range(1000, 9999);
    }
}