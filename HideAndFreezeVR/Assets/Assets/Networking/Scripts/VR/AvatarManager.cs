using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Easily accesibly database for avatars.
/// </summary>
public class AvatarManager : MonoBehaviour {

    public static AvatarManager Instance;

    public List<GameObject> Avatars;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    /*public GameObject GetRandomAvatar()
    {
        int number = Random.Range(0, Avatars.Count);

        return Avatars[number];
    }*/
}
