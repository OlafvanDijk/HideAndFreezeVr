using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour {

    public static AvatarManager Instance;

    public List<GameObject> Avatars;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public GameObject GetRandomAvatar()
    {
        int number = Random.Range(0, Avatars.Count);

        return Avatars[number];
    }
}
