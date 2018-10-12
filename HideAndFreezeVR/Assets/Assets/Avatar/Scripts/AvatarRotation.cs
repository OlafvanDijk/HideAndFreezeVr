using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRotation : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

    private GameObject player;
    private VRAvatarController avatarController;
    private Transform transformPlayer;

    private bool Oculus = true;

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitForContainer());
    }

    private IEnumerator WaitForContainer()
    {
        yield return new WaitForSeconds(1f);
        avatarController = GetComponent<VRAvatarController>();
        player = avatarController.containerObject;
        
        yield return new WaitUntil(() => player != null);
        this.transformPlayer = player.transform;
    }

    // Update is called once per frame
    void Update() {
        if (Oculus)
        {
            try
            {
                if (transformPlayer != null)
                {
                    float newXRotation = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
                    if (newXRotation != 0)
                    {
                        transformPlayer.RotateAround(avatarController.transform.position, new Vector3(0, 1, 0), newXRotation * rotationSpeed);
                    }
                }
            }
            catch (System.Exception)
            {
                Oculus = false;
            }
        }
    }
}
