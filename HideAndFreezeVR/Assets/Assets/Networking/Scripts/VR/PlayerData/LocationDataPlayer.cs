using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDataPlayer : MonoBehaviour {

    public Transform leftHand;
    public Transform rightHand;
    public Transform head;

    private void Awake()
    {
        Debug.Log("player");
        VR_PlayerNetwork.Instance.player = this;
        //DontDestroyOnLoad(this.transform.parent.gameObject);
    }

    private void Start()
    {
    }
}
